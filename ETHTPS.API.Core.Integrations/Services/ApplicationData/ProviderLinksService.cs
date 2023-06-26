﻿using ETHTPS.Data.Integrations.MSSQL;

namespace ETHTPS.API.Core.Integrations.MSSQL.Services.ApplicationData
{
    public sealed class ProviderLinksService : EFCoreCRUDServiceBase<ProviderLink>
    {
        private readonly ExternalWebsitesService _externalWebsitesService;
        private readonly ExternalWebsiteCategoryService _externalWebsiteCategoryService;

        public ProviderLinksService(EthtpsContext context,
            ExternalWebsiteCategoryService externalWebsiteCategoryService,
            ExternalWebsitesService externalWebsitesService) : base(
            context.ProviderLinks ?? throw new ArgumentNullException("ProviderLinks"), context)
        {
            _externalWebsiteCategoryService = externalWebsiteCategoryService;
            _externalWebsitesService = externalWebsitesService;
        }

        public override void Create(ProviderLink entity)
        {
            var host = (new Uri(entity.Link ?? throw new ArgumentNullException(nameof(entity.Link)))).Host;
            if (!_externalWebsitesService.GetAll()?.Any(x => x.Id == entity.ExternalWebsiteId || x.Name == host) ?? true)
            {
                var othersID = _externalWebsiteCategoryService.GetAll()?.First(x => x.Name == "Others").Id ?? -1;
                _externalWebsitesService.Create(new ExternalWebsite()
                {
                    Category = othersID,
                    Name = host,
                    IconBase64 =
                        "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAgAAAAIACAYAAAD0eNT6AAAZf0lEQVR4Xu3WOw6lRxWFUZyRgMQ0EIwGJHsmzIGR2BKMBgKGgSNSbuiAR+BTe3drL0ctt/qvqnXOlb5vfuE/AgQIECBAYE7gm7kXezABAgQIECDwCwFgCQgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBAgMCggAAYHLonEyBAgAABAWAHCBAgQIDAoIAAGBy6JxMgQIAAAQFgBwgQIECAwKCAABgcuicTIECAAAEBYAcIECBA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"
                });
                var websiteID = _externalWebsitesService.GetAll()?.First(x => x.Category == othersID && x.Name == host)
                    .Id;
                entity.ExternalWebsiteId = websiteID ?? -1;
            }
            else
            {
                entity.ExternalWebsiteId = _externalWebsitesService.GetAll()
                    ?.First(x => x.Id == entity.ExternalWebsiteId || x.Name == host).Id ?? -1;
            }
            base.Create(entity);
        }
    }
}