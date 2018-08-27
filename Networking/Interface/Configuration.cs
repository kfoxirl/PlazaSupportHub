///////////////////////////////////////////////////////////////////////////////
//
// File           : $Workfile: Configuration.cs $
// Version        : $Revision: 1 $
// Function       : 
//
// Author         : $Author: Len $
// Date           : $Date: 11/07/02 15:50 $
//
// Notes          : 
//
// Modifications  :
//
// $Log: /Web Articles/SocketServerTest/SocketServerTest.Interface/Configuration.cs $
// 
// 1     11/07/02 15:50 Len
// 
///////////////////////////////////////////////////////////////////////////////
//
// Copyright 1997 - 2002 JetByte Limited.
//
// JetByte Limited grants you ("Licensee") a non-exclusive, royalty free, 
// licence to use, modify and redistribute this software in source and binary 
// code form, provided that i) this copyright notice and licence appear on all 
// copies of the software; and ii) Licensee does not utilize the software in a 
// manner which is disparaging to JetByte Limited.
//
// This software is provided "as is" without a warranty of any kind. All 
// express or implied conditions, representations and warranties, including
// any implied warranty of merchantability, fitness for a particular purpose
// or non-infringement, are hereby excluded. JetByte Limited and its licensors 
// shall not be liable for any damages suffered by licensee as a result of 
// using, modifying or distributing the software or its derivatives. In no
// event will JetByte Limited be liable for any lost revenue, profit or data,
// or for direct, indirect, special, consequential, incidental or punitive
// damages, however caused and regardless of the theory of liability, arising 
// out of the use of or inability to use software, even if JetByte Limited 
// has been advised of the possibility of such damages.
//
// This software is not designed or intended for use in on-line control of 
// aircraft, air traffic, aircraft navigation or aircraft communications; or in 
// the design, construction, operation or maintenance of any nuclear 
// facility. Licensee represents and warrants that it will not use or 
// redistribute the Software for such purposes. 
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Xml;

namespace PlazaConnectivityChecker.Interface
{
	public enum Protocol 
	{ 
		TCP, 
		UDP 
	};

	public class Configuration
	{
		public Configuration(XmlNode root)
		{
			this.root = root;
		}

		public static XmlNode GetNodeFromFile(string xmlFileName, string nodeName)
		{
			XmlDocument xmlDocument = new XmlDocument();

			xmlDocument.Load(xmlFileName);
				
			return xmlDocument.SelectSingleNode(nodeName);
		}

		protected int GetInt(string name)
		{
			XmlNode node = root.SelectSingleNode(name);

			return Int32.Parse(node.InnerText);
		}

		protected int GetOptionalInt(string name, int defaultValue)
		{
			int result = defaultValue;
			
			XmlNode node = root.SelectSingleNode(name);

			if (node != null)
			{
				string val = node.InnerText;

				if (val != null && val != "")
				{
					result = Int32.Parse(val);
				}
			}

			return result;
		}

		protected bool GetOptionalBool(string name, bool defaultValue)
		{
			bool result = defaultValue;

			XmlNode node = root.SelectSingleNode(name);

			if (node != null)
			{
				string val = node.InnerText;

				if (val != null && val != "")
				{
					result = Boolean.Parse(val);
				}
			}

			return result;
		}

		protected string GetString(string name)
		{
			XmlNode node = root.SelectSingleNode(name);

			return node.InnerText;
		}

		protected string GetOptionalString(string name, string defaultValue)
		{
			string result = defaultValue;

			XmlNode node = root.SelectSingleNode(name);

			if (node != null)
			{
				string val = node.InnerText;

				if (val != null)
				{
					result = val;
				}
			}

			return result;
		}
		
		protected XmlNode root;
	}
}

///////////////////////////////////////////////////////////////////////////////
// End of file...
///////////////////////////////////////////////////////////////////////////////
