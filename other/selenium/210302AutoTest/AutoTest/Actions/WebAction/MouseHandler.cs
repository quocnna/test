using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;
using System.Threading;
using AutoTest.Data;

namespace AutoTest.WebAction
{
    internal static class MouseHandler
    {
        public static void Click(IWebElement webEle, IMemory stepData)
        {
            int x = (int)stepData[ParaInfo.XCoord.Name];
            int y = (int)stepData[ParaInfo.YCoord.Name];
            if (x > 0 || y > 0)
            {
                Actions action = new Actions(WebAction.CurrentWebDriver);
                action.MoveToElement(webEle, x, y).Click().Release(webEle).Build().Perform();
            }
            else
                webEle.Click();
        }

        public static void DoubleClick(IWebElement webEle)
        {
            Actions action = new Actions(WebAction.CurrentWebDriver);
            action.DoubleClick();
            action.Perform();
        }

        public static void MouseOver(IWebElement webEle)
        {
            Actions action = new Actions(WebAction.CurrentWebDriver);
            action.MoveToElement(webEle).MoveByOffset(5, 5).ClickAndHold().Release().Build().Perform();
        }

        public static void DragAndDrop(IWebElement webSourceEle, IMemory stepData)
        {
            Value method = stepData[ParaInfo.FindTargetBy.Name];
            FindBy findTargetBy = method.IsEmpty() ? FindBy.Id : (FindBy)Enum.Parse(typeof(FindBy), (string)method);
            string valueTargetFindToFind = (string)stepData[ParaInfo.ObjectTargetId.Name];
            int x = (int)stepData[ParaInfo.XCoord.Name];
            int y = (int)stepData[ParaInfo.YCoord.Name];


            By by = findTargetBy == FindBy.Id ? By.Id(valueTargetFindToFind) :
                   findTargetBy == FindBy.Name ? By.Name(valueTargetFindToFind) :
                   findTargetBy == FindBy.TagName ? By.TagName(valueTargetFindToFind) :
                   findTargetBy == FindBy.ClassName ? By.ClassName(valueTargetFindToFind) :
                   findTargetBy == FindBy.CssSelector ? By.CssSelector(valueTargetFindToFind) :
                   findTargetBy == FindBy.XPath ? By.XPath(valueTargetFindToFind) :
                   findTargetBy == FindBy.LinkText ? By.LinkText(valueTargetFindToFind) : By.PartialLinkText(valueTargetFindToFind);
            IWebElement webDestEle = WebAction.CurrentWebDriver.FindElement(by);

            Actions action = new Actions(WebAction.CurrentWebDriver);
            action.ClickAndHold(webSourceEle).MoveToElement(webDestEle, x, y).Release(webDestEle).Build().Perform();
        }
    }
}
