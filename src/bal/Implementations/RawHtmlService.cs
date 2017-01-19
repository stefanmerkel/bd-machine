﻿namespace bd.machine.bal.Implementations
{
	using System;
	using Models;
	using dal.Implementations.Models;
	using dal.Interfaces;
	using Interfaces;
	using System.Linq;
	using System.Text;
	using HtmlAgilityPack;
	using bal.Models.Htmltags;
	using System.Collections.Generic;

	public class RawHtmlService : IRawHtmlService
	{
		private readonly IUnitOfWork _unitOfWork;
		
		public RawHtmlService(IUnitOfWork unitOfWork)
		{
			if (unitOfWork == null)
				throw new ArgumentNullException("unitOfWork");
			_unitOfWork = unitOfWork;
		}

		public IEnumerable<HtmlMetaTag> GetHtmlMetaTagsFromRawHtml(string rawHtml)
		{
			if (string.IsNullOrWhiteSpace(rawHtml))
				throw new ArgumentNullException("rawHtml");
			var htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(rawHtml);
			var metaNodes = htmlDocument.DocumentNode.SelectNodes("//meta");
			if (metaNodes.Count == 0)
				return new List<HtmlMetaTag>();
			return metaNodes
				.Select(x => new HtmlMetaTag()
				{
					Name = x.GetAttributeValue("name", ""),
					Content = x.GetAttributeValue("content", "")
				});
		}

		public string GetHtmlTitleFromRawHtml(string rawHtml)
		{
			if (string.IsNullOrWhiteSpace(rawHtml))
				throw new ArgumentNullException("rawHtml");
			var htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(rawHtml);
			var titleNode = htmlDocument.DocumentNode.SelectSingleNode("//title");
			if (titleNode == null)
				return string.Empty;
			return titleNode.InnerHtml;
		}

		public RawHtmlServiceModel GetRawHtmlRawHtmlServiceModelById(int rawHtmlId)
		{
			if (rawHtmlId <= 0)
				throw new ArgumentOutOfRangeException("rawHtmlId");
			var urlRawHtmlEntry = _unitOfWork
				.UrlRawHtmlRepository
				.UrlRawHtml
				.Single(x => x.RawHtmlId == rawHtmlId);
			return new RawHtmlServiceModel()
			{
				RawHtmlId = urlRawHtmlEntry.RawHtmlId,
				UrlId = urlRawHtmlEntry.UrlId,
				DownloadDateTime = urlRawHtmlEntry.CreationDate,
				RawHtml = Encoding.Default.GetString(urlRawHtmlEntry.RawHtml.Value),
			};
		}

		public void SaveRawHtmlAsByteArray(byte[] rawHtml, int urlId)
		{
			if (rawHtml == null)
				throw new ArgumentNullException("rawHtml");
			if (urlId <= 0)
				throw new ArgumentOutOfRangeException("urlId");

			var rawHtmlEntry = new RawHtml();
			rawHtmlEntry.Value = rawHtml;
			_unitOfWork.RawHtmlRepository.Create(rawHtmlEntry);
			_unitOfWork.UrlRawHtmlRepository.Create(new UrlRawHtml() 
			{
				UrlId = urlId,
				RawHtmlId = rawHtmlEntry.Id,
				CreationDate = DateTime.Now
			});
		}
	}
}
