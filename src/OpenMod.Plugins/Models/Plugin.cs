﻿using System;
using System.Collections.Generic;
using System.Linq;
using BaGet.Protocol.Models;

namespace OpenMod.Plugins.Models
{
    public record Plugin(
        string Id,
        string Title,
        string Description,
        IReadOnlyList<string> Authors,
        string? SiteUrl)
    {
        public Plugin(SearchResult searchResult) : this(
            searchResult.PackageId,
            searchResult.Title,
            FixDescription(searchResult.Description),
            FixAuthors(searchResult.Authors),
            searchResult.ProjectUrl)
        {
        }

        public bool IsOfficial { get; }
            = Id.StartsWith("OpenMod.", StringComparison.OrdinalIgnoreCase)
              && Authors.Any(x => x == Id || x.Trim().Equals("OpenMod", StringComparison.OrdinalIgnoreCase));

        public string CommandInstall { get; } = "openmod install " + Id;

        public string NuGetUrl { get; } = "https://www.nuget.org/packages/" + Id;

        private static string FixDescription(string description)
        {
            return description.Trim() == "Package Description" ? "" : description;
        }

        private static IReadOnlyList<string> FixAuthors(IReadOnlyList<string> authors)
        {
            return authors
                .SelectMany(x => x
                    .Split(',')
                    .Select(y => y.Trim()))
                .ToList();
        }
    }
}