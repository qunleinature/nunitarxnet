// ****************************************************************
// This is free software licensed under the NUnit license. You
// may obtain a copy of the license as well as information regarding
// copyright ownership at http://nunit.org.
// ****************************************************************

// ****************************************************************
// Copyright 2012, Lei Qun 
// 2012.12.20修改
// ****************************************************************

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

using NUnit.UiKit;
using NUnit.Util.ArxNet;
using NUnit.Util;

namespace NUnit.UiKit.ArxNet
{
    public class TestTreeArxNet : TestTree
    {
        #region Instance Variables

        // Our test loader
        //CAD环境下的TestLoaderArxNet
        private TestLoaderArxNet loader = null;

        #endregion

        #region Construction and Initialization

		public TestTreeArxNet() : base()
		{
            
		}

		protected override void OnLoad(EventArgs e)
		{            
            /*
             if ( !this.DesignMode )
			{
				this.ShowCheckBoxes = 
					Services.UserSettings.GetSetting( "Options.ShowCheckBoxes", false );
				Initialize( Services.TestLoader );
				Services.UserSettings.Changed += new SettingsEventHandler(UserSettings_Changed);
			}

			base.OnLoad (e);
             */
            base.OnLoad(e);

            if ( !this.DesignMode )
            {
                this.ShowCheckBoxes =
                    Services.UserSettings.GetSetting("Options.ShowCheckBoxes", false);
                TestLoaderArxNet loaderArxNet = Services.TestLoader as TestLoaderArxNet;
                Initialize(loaderArxNet);
                Services.UserSettings.Changed += new SettingsEventHandler(UserSettings_Changed);
            }
            
            Type type = typeof(UserControl);
            MethodInfo method = type.GetMethod("OnLoad", BindingFlags.Instance | BindingFlags.NonPublic);
            if (method != null)
            {
                object[] param = new object[] { e };
                UserControl userControl = this as UserControl;
                
                method.Invoke(userControl, param);
            }
        }

		public void Initialize(TestLoaderArxNet loader) 
		{
            /*
            this.tests.Initialize(loader, loader.Events);
			this.loader = loader;
			loader.Events.TestLoaded += new NUnit.Util.TestEventHandler(events_TestLoaded);
			loader.Events.TestReloaded += new NUnit.Util.TestEventHandler(events_TestReloaded);
			loader.Events.TestUnloaded += new NUnit.Util.TestEventHandler(events_TestUnloaded);
            */
            
            object value;
            TestSuiteTreeView tests;
            //TestTree：private NUnit.UiKit.TestSuiteTreeView tests;
            //this.tests.Initialize(loader, loader.Events);
            tests = null;
            value = GetBaseNoPublicField("tests");
            if (value != null) tests = (TestSuiteTreeView)value;
            if (tests != null) tests.Initialize(loader, loader.Events);

            this.loader = loader;
            loader.Events.TestLoaded += new NUnit.Util.TestEventHandler(events_TestLoaded);
            loader.Events.TestReloaded += new NUnit.Util.TestEventHandler(events_TestReloaded);
            loader.Events.TestUnloaded += new NUnit.Util.TestEventHandler(events_TestUnloaded);
		}		

		#endregion

        #region Helper Methods        

        //获得基类对象的私有字段的值
        private object GetBaseNoPublicField(string fieldName)
        {
            try
            {
                //利用反射访问基类对象的私有字段
                Type baseType = this.GetType().BaseType;
                FieldInfo field = baseType.GetField(fieldName, BindingFlags.Instance | BindingFlags.GetField | BindingFlags.IgnoreCase | BindingFlags.NonPublic);
                object value = field.GetValue(this);
                return value;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        //设置基类对象的私有字段的值
        private void SetBaseNoPublicField(string fieldName, object value)
        {
            try
            {
                //利用反射访问基类对象的私有字段
                Type baseType = this.GetType().BaseType;
                FieldInfo field = baseType.GetField(fieldName, BindingFlags.Instance | BindingFlags.SetField | BindingFlags.IgnoreCase | BindingFlags.NonPublic);
                if (field != null) field.SetValue(this, value);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        //运行基类对象的私有函数
        private object CallBaseNoPublicMethod(string methodName, object[] param)
        {
            try
            {
                //利用反射访问基类对象的私有函数
                Type baseType = this.GetType().BaseType;
                MethodInfo method = baseType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                object result = method.Invoke(this, param);
                return result;
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        #endregion

        private void events_TestLoaded(object sender, TestEventArgs args)
        {
            object[] param;

            param = new object[] { sender, args };
            CallBaseNoPublicMethod("events_TestLoaded", param);//调用基类方法           
        }

        private void events_TestReloaded(object sender, TestEventArgs args)
        {
            object[] param;

            param = new object[] { sender, args };
            CallBaseNoPublicMethod("events_TestReloaded", param);//调用基类方法
        }

        private void events_TestUnloaded(object sender, TestEventArgs args)
        {
            object[] param;

            param = new object[] { sender, args };
            CallBaseNoPublicMethod("events_TestReloaded", param);//调用基类方法
        }

        private void UserSettings_Changed(object sender, SettingsEventArgs args)
        {
            object[] param;

            param = new object[] { sender, args };
            CallBaseNoPublicMethod("UserSettings_Changed", param);//调用基类方法
        }

    }
}
