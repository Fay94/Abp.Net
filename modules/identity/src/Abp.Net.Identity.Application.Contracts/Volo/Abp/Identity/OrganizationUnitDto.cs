﻿using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity
{
    public class OrganizationUnitDto : ExtensibleFullAuditedEntityDto<Guid>, IMultiTenant, IHasConcurrencyStamp
    {
        public OrganizationUnitDto()
        {
            Children = new List<OrganizationUnitDto>();
        }

        public Guid? TenantId { get; set; }

        public virtual Guid? ParentId { get; set; }

        public virtual string Code { get; set; }

        public virtual string DisplayName { get; set; }

        public int Level => GetLevel();

        public virtual bool IsLeaf { get; set; }

        public string ConcurrencyStamp { get; set; }

        public List<OrganizationUnitDto> Children { get; set; }

        private int GetLevel()
        {
            return Code.Split('.').Length;
        }

        public void SetLeaf()
        {
            IsLeaf = true;
        }
    }
}
