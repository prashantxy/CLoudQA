using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

class CloudQATest
{
    static void Main()
    {
        IWebDriver driver = new ChromeDriver();
        driver.Navigate().GoToUrl("https://app.cloudqa.io/home/AutomationPracticeForm");
        driver.Manage().Window.Maximize();
        System.Threading.Thread.Sleep(3000); 

        try
        {
            // Step 1: Switch to the first iframe
            IWebElement iframe = driver.FindElement(By.TagName("iframe"));
            driver.SwitchTo().Frame(iframe);

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            // Step 2: Access nested Shadow DOM
            string shadowRootScript = @"
                const root1 = document.querySelector('form-container').shadowRoot;
                const root2 = root1.querySelector('form-element').shadowRoot;
                return root2;
            ";

            // Field 1: First Name (test: entering text)
            IWebElement firstNameInput = (IWebElement)js.ExecuteScript(shadowRootScript + @"
                return arguments[0].querySelector('input[placeholder=""Name""]');
            ", driver);
            firstNameInput.SendKeys("TestFirst");

            // Field 2: Gender Radio Button (test: selecting radio)
            IWebElement maleRadio = (IWebElement)js.ExecuteScript(shadowRootScript + @"
                return arguments[0].querySelector('input[value=""Male""]');
            ", driver);
            maleRadio.Click();

            // Field 3: Description Textarea (test: entering description)
            IWebElement description = (IWebElement)js.ExecuteScript(shadowRootScript + @"
                return arguments[0].querySelectorAll('textarea')[1];
            ", driver);
            description.SendKeys("This is a test description.");

            Console.WriteLine("✅ All three fields tested successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Test failed: " + ex.Message);
        }
        finally
        {
            System.Threading.Thread.Sleep(3000);
            driver.Quit();
        }
    }
}
