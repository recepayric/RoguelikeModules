using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MVC.Base.Runtime.Utility
{
	public static class CsvReader
	{
		static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
		static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
		static char[] TRIM_CHARS = {'\"'};
		
		private static string UnquoteString(string str)
		{
			if (string.IsNullOrEmpty(str))
			return str;
			int length = str.Length;
			if (length > 1 && str[0] == '\"' && str[length-1] == '\"')
			str = str.Substring(1, length-2);
			return str;
		}
		
		public static List<Dictionary<string, object>> ReadCSV(string file)
		{
			TextAsset data = new TextAsset(System.IO.File.ReadAllText(file));
			return ReadCSVText(data.text);
		}
		
		public static List<Dictionary<int, object>> ReadCSVRaw(string file)
		{
			TextAsset data = new TextAsset(System.IO.File.ReadAllText(file));
			return ReadCSVRawText(data.text);
		}
		
		public static List<Dictionary<string, object>> ReadCSVText(string text)
		{
			var list = new List<Dictionary<string, object>>();
			var lines = Regex.Split(text, LINE_SPLIT_RE);
			if (lines.Length <= 1) return null;
			var header = Regex.Split(lines[0], SPLIT_RE);
			for (var i = 1; i < lines.Length; i++)
			{
				var values = Regex.Split(lines[i], SPLIT_RE);
				if (values.Length == 0 || values[0] == "") continue;
				var entry = new Dictionary<string, object>();
				for (var j = 0; j < header.Length && j < values.Length; j++)
				{
					//string value = values[j];
					//value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
					string value = values[j].Replace("\"\"","\"");
					value = UnquoteString(value);
					object finalvalue = value;
					/*int n;
					float f;
					if (int.TryParse(value, out n))
					{
						finalvalue = n;
					}
					else if (float.TryParse(value, out f))
					{
						finalvalue = f;
					}*/
					entry[header[j]] = finalvalue;
				}
				list.Add(entry);
			}
			return list;
		}
		
		public static List<Dictionary<int, object>> ReadCSVRawText(string text)
		{
			var list = new List<Dictionary<int, object>>();
			var lines = Regex.Split(text, LINE_SPLIT_RE);
			if (lines.Length <= 1) return null;
			for (var i = 0; i < lines.Length; i++)
			{
				var values = Regex.Split(lines[i], SPLIT_RE);
				if (values.Length == 0 || values[0] == "") continue;
				var entry = new Dictionary<int, object>();
				for (var j = 0; j < values.Length; j++)
				{
					//string value = values[j];
					//value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
					string value = values[j].Replace("\"\"","\"");
					value = UnquoteString(value);
					object finalvalue = value;
					/*int n;
					float f;
					if (int.TryParse(value, out n))
					{
						finalvalue = n;
					}
					else if (float.TryParse(value, out f))
					{
						finalvalue = f;
					}*/
					entry[j] = finalvalue;
				}
				list.Add(entry);
			}
			return list;
		}
	}
}