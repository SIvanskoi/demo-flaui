using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using FLaUIDemo.Common;


namespace FLaUIDemo.COM
{
    public class ComInterop
    {
        static readonly string c_ProgId = $"{Configuration.ProcessName}.Application";

        static readonly string c_publicKeyToken = "a1ed4762827d08d4";

        /// <summary>
        /// Returns Application COM object.
        /// </summary>
        public static dynamic Application
        {
            get
            {
                try
                {
                    Type comType = Type.GetTypeFromProgID(c_ProgId);
                    dynamic appObject = Activator.CreateInstance(comType);
                    return appObject;
                }
                catch (COMException)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Returns map object bounding rectangle in screen coordinates.
        /// </summary>
        public static Rectangle? GetObjectRect(dynamic obj)
        {
            if ( IsTopic(obj) )
            {
                obj.GetBoundingRect(out float fTop, out float fLeft, out float fWidth, out float fHeight);
                float fBottom = fTop + fHeight;
                float fRight = fLeft + fWidth;
                obj.Document.Window.MapToScreenPosition(fLeft, fTop, out int iLeft, out int iTop);
                obj.Document.Window.MapToScreenPosition(fRight, fBottom, out int iRight, out int iBottom);
                return new Rectangle(iLeft, iTop, iRight - iLeft, iBottom - iTop);
            }
            return null;
        }

        public static Type GetType(string typeName)
        {
            Assembly assembly = Assembly.Load($"Mindjet.{Configuration.ProcessName}.Interop, Version={Configuration.Version}.0.0.0, Culture=neutral, PublicKeyToken={c_publicKeyToken}");
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (type.Name == typeName)
                {
                    return type;
                }
            }
            return null;
        }

        public static bool IsBackgroundObject(dynamic obj)
        {
            return obj.Type == 4;
        }

        public static bool IsDataModelObjectsSet(dynamic obj)
        {
            return obj.Type == 5;
        }

        public static bool IsTopic(dynamic obj)
        {
            //Type type = GetType("MmDocumentObjectType");
            //Array values = Enum.GetValues(type);
            //return obj.Type == (int)values.GetValue(1);
            return obj.Type == 1;
        }

        public static void SafeRelease(object comObject)
        {
            try
            {
                if (comObject != null && Marshal.IsComObject(comObject))
                    Marshal.FinalReleaseComObject(comObject);
            }
            catch { /* ignore */ }
        }

    }
}
