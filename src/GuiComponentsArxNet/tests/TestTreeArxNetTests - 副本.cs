// ****************************************************************
// Copyright 2010, Charlie Poole
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org
// ****************************************************************

// ****************************************************************
// Copyright 2012, Lei Qun 
// 2012.12.21修改
// 2012.6.6修改
//   1.改成在NUnit2.6.2基础
// ****************************************************************

using System;
using System.Collections;
using System.Reflection;

using NUnit.Framework;
using NUnit.Util;
using NUnit.UiKit;
using NUnit.Util.ArxNet;
using NUnit.UiKit.ArxNet;

namespace NUnit.UiKit.ArxNet.Tests
{
	[TestFixture]
	public class TestTreeArxNetTests
	{
		[Test]
		public void SameCategoryShouldNotBeSelectedMoreThanOnce()
		{
			// arrange
			TestTreeArxNet target = new TestTreeArxNet();

			// we need to populate the available categories
			// this can be done via TestLoader but this way the test is isolated
            FieldInfo fieldInfo = typeof(TestTree).GetField("availableCategories", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(fieldInfo, "The field 'availableCategories' should be found.");
            object fieldValue = fieldInfo.GetValue(target);
            Assert.IsNotNull(fieldValue, "The value of 'availableCategories' should not be null.");
            IList availableCategories = fieldValue as IList;
            Assert.IsNotNull(availableCategories, "'availableCategories' field should be of type IList.");

			string[] expectedSelectedCategories = new string[] { "Foo", "MockCategory" };
			foreach (string availableCategory in expectedSelectedCategories)
			{
				availableCategories.Add(availableCategory);
			}

			// act
			target.SelectCategories(expectedSelectedCategories, true);
			target.SelectCategories(expectedSelectedCategories, true);
			string[] actualSelectedCategories = target.SelectedCategories;

			// assert
			CollectionAssert.AreEquivalent(expectedSelectedCategories, actualSelectedCategories);
		}

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
	}
}
