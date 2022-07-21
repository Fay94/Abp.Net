using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.OpenApi.Models;
using RazorEngine.Templating;
using Spire.Doc;
using Spire.Doc.Documents;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;

namespace Abp.Net.Swashbuckle.Export
{
    [Area("Abp")]
    [Route("Abp/Swagger")]
    [DisableAuditing]
    [RemoteService(false)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SwaggerExportController : AbpController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly SwaggerGenerator _swaggerGenerator;

        public SwaggerExportController(SwaggerGenerator swaggerGenerator, IWebHostEnvironment webHostEnvironment)
        {
            _swaggerGenerator = swaggerGenerator;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// 导出文件
        /// </summary>
        /// <param name="type">文件类型</param>
        /// <param name="version">版本号</param>
        /// <returns></returns>
        [HttpGet("export")]
        public FileResult Export(string type, string version)
        {
            var model = _swaggerGenerator.GetSwagger(version);
            var docs = ResolveDocument(model);
            var html = GeneritorSwaggerHtmlNew($"{_webHostEnvironment.WebRootPath}\\SwaggerDoc.cshtml", docs);
            var outdata = SwaggerConversHtml(html, type, out string contenttype);
            return File(outdata, contenttype, $"API文档 {version}{type}");
        }

        private List<SwaggerDoc> ResolveDocument(OpenApiDocument model)
        {
            List<SwaggerDoc> docs = new();
            foreach (var path in model.Paths)
            {
                foreach (var operation in path.Value.Operations)
                {
                    SwaggerDoc doc = new();
                    doc.Url = path.Key;
                    doc.Method = operation.Key.ToString();
                    doc.Summary = operation.Value.Summary;
                    // Parameters
                    if (operation.Value.Parameters.Any())
                    {
                        doc.QueryParameters = new List<SwaggerParam>();
                        foreach (var param in operation.Value.Parameters)
                        {
                            doc.QueryParameters.Add(new SwaggerParam
                            {
                                Name = param.Name,
                                Description = param.Description,
                                Type = param.Schema?.Type,
                                IsRequired = param.Required
                            });
                        }
                    }
                    // RequestBody
                    if (operation.Value.RequestBody != null)
                    {
                        doc.BodyParameters = new List<SwaggerParam>();
                        var bodySchema = operation.Value.RequestBody.Content.Values.FirstOrDefault().Schema;
                        if (bodySchema.Reference != null)
                        {
                            var referenceId = bodySchema.Reference.Id;
                            bodySchema = model.Components.Schemas[bodySchema.Reference.Id];
                            ResolveProperty(model.Components, doc.BodyParameters, referenceId, bodySchema);
                        }
                        else if (bodySchema.Type != null && bodySchema.Type.Equals("object"))
                        {
                            ResolveProperty(model.Components, doc.BodyParameters, null, bodySchema);
                        }
                    }

                    var response = operation.Value.Responses.FirstOrDefault().Value;
                    if (response.Content.Values.Count > 0)
                    {
                        doc.ResParameters = new List<SwaggerParam>();
                        var responseSchema = response.Content.Values.FirstOrDefault().Schema;
                        if (responseSchema.Reference == null)
                        {
                            continue;
                        }
                        var referenceId = responseSchema.Reference.Id;
                        responseSchema = model.Components.Schemas[responseSchema.Reference.Id];
                        ResolveProperty(model.Components, doc.ResParameters, referenceId, responseSchema);
                    }
                    else
                    {
                    }
                    docs.Add(doc);
                }
            }
            return docs;
        }

        private void ResolveProperty(OpenApiComponents components, List<SwaggerParam> paramList, string referenceId, OpenApiSchema schema, int layerid = 0, string str = "")
        {
            if (paramList.Any(p => p.LayerId < layerid && p.ReferenceId.Equals(referenceId)))
            {
                return;
            }
            foreach (var property in schema.Properties)
            {
                var param = new SwaggerParam
                {
                    LayerId = layerid,
                    ReferenceId = referenceId,
                    Name = str + property.Key,
                    Description = property.Value.Description,
                    Type = property.Value.Type,
                    IsRequired = schema.Required.Any(p => p.Equals(property.Key))
                };
                if (param.Name.Equals("code"))
                {
                    param.Description = "代码";
                }
                else if (param.Name.Equals("message"))
                {
                    param.Description = "消息";
                }
                else if (param.Name.Equals("data"))
                {
                    param.Description = "数据";
                }
                else if (param.Name.Equals("state"))
                {
                    param.Description = "状态";
                }
                paramList.Add(param);

                if (property.Value.Reference != null)
                {
                    int temp = layerid + 1;
                    string temp1 = "";
                    if (string.IsNullOrWhiteSpace(str)) temp1 = "├─";
                    else temp1 = "" + str;
                    ResolveProperty(components, paramList, property.Value.Reference.Id, components.Schemas[property.Value.Reference.Id], temp, temp1);
                }
                else if (property.Value.Type != null && property.Value.Type.Equals("array"))
                {
                    if (property.Value.Items.Reference != null)
                    {
                        int temp = layerid + 1;
                        string temp1 = "";
                        if (string.IsNullOrWhiteSpace(str)) temp1 = "├─";
                        else temp1 = "" + str;
                        ResolveProperty(components, paramList, property.Value.Items.Reference.Id, components.Schemas[property.Value.Items.Reference.Id], temp, temp1);
                    }
                }
            }
        }

        /// <summary>
        /// 将数据遍历静态页面中
        /// </summary>
        /// <param name="templatePath">静态页面地址</param>
        /// <param name="model">获取到的文件数据</param>
        /// <returns></returns>
        private static string GeneritorSwaggerHtmlNew(string templatePath, List<SwaggerDoc> model)
        {
            var template = System.IO.File.ReadAllText(templatePath);
            var result = RazorEngine.Engine.Razor.RunCompile(template, "Furion", typeof(List<SwaggerDoc>), model);
            return result;
        }

        /// <summary>
        /// 静态页面转文件
        /// </summary>
        /// <param name="html">静态页面html</param>
        /// <param name="type">文件类型</param>
        /// <param name="contenttype">上下文类型</param>
        /// <returns></returns>
        private Stream SwaggerConversHtml(string html, string type, out string contenttype)
        {
            string fileName = Guid.NewGuid().ToString() + type;
            //文件存放路径
            string webRootPath = _webHostEnvironment.WebRootPath;
            string path = webRootPath + @"\Files\TempFiles\";
            var addrUrl = path + $"{fileName}";
            FileStream fileStream = null;
            var provider = new FileExtensionContentTypeProvider();
            contenttype = provider.Mappings[type];
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var data = Encoding.Default.GetBytes(html);
                var stream = ByteHelper.BytesToStream(data);
                //创建Document实例
                Document document = new Document();
                //加载HTML文档
                document.LoadFromStream(stream, FileFormat.Html, XHTMLValidationType.None);
                switch (type)
                {
                    case ".docx":
                        //Word
                        document.SaveToFile(addrUrl, FileFormat.Docx);
                        break;

                    case ".pdf":
                        //PDF
                        document.SaveToFile(addrUrl, FileFormat.PDF);
                        break;

                    case ".html":
                        //Html
                        FileStream fs = new FileStream(addrUrl, FileMode.Append, FileAccess.Write, FileShare.None);//html直接写入不用spire.doc
                        StreamWriter sw = new StreamWriter(fs); // 创建写入流
                        sw.WriteLine(html); // 写入Hello World
                        sw.Close(); //关闭文件
                        fs.Close();
                        break;

                    case ".xml":
                        //PDF
                        document.SaveToFile(addrUrl, FileFormat.WordXml);
                        break;

                    case ".svg":
                        //PDF
                        document.SaveToFile(addrUrl, FileFormat.SVG);
                        break;
                }

                document.Close();
                fileStream = System.IO.File.Open(addrUrl, FileMode.OpenOrCreate);
                var filedata = ByteHelper.StreamToBytes(fileStream);
                var outdata = ByteHelper.BytesToStream(filedata);

                return outdata;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
                if (System.IO.File.Exists(addrUrl))
                    System.IO.File.Delete(addrUrl);//删掉文件
            }
        }
    }

    public class ByteHelper
    {
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// 将 byte[] 转成 Stream
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
    }

    public class SwaggerDoc
    {
        /// <summary>
        /// 接口地址
        /// </summary>
        public string Url { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 接口描述
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Get请求参数
        /// </summary>
        public List<SwaggerParam> QueryParameters { get; set; }

        /// <summary>
        /// Post请求参数
        /// </summary>
        public List<SwaggerParam> BodyParameters { get; set; }

        /// <summary>
        /// 返回参数
        /// </summary>
        public List<SwaggerParam> ResParameters { get; set; }
    }

    public class SwaggerParam
    {
        public int LayerId { get; set; }

        public string ReferenceId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public bool IsRequired { get; set; }

        public string DefaultValue { get; set; }
    }
}