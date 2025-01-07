using Azure.Core;
using Azure.ResourceManager.PlaywrightTesting.Models;
using Azure.ResourceManager.PlaywrightTesting;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager;
using Azure;
using Azure.Identity;

namespace cpsdktesting
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            await AccountsCreateOrUpdate();
            //await AccountDelete();
        }

        public static async Task AccountsCreateOrUpdate()
        {
            // Generated from example definition: 2024-12-01/Accounts_CreateOrUpdate.json
            // this example is just showing the usage of "Account_CreateOrUpdate" operation, for the dependent resources, they will have to be created separately.

            // get your azure access token, for more details of how Azure SDK get your access token, please refer to https://learn.microsoft.com/en-us/dotnet/azure/sdk/authentication?tabs=command-line
            TokenCredential cred = new DefaultAzureCredential();
            // authenticate your client
            ArmClient client = new(cred);

            // this example assumes you already have this ResourceGroupResource created on azure
            // for more information of creating ResourceGroupResource, please refer to the document of ResourceGroupResource
            string subscriptionId = "9fd9eff4-f386-452e-9893-06417ff6e808";
            string resourceGroupName = "bugtest-rg";
            ResourceIdentifier resourceGroupResourceId = ResourceGroupResource.CreateResourceIdentifier(subscriptionId, resourceGroupName);
            ResourceGroupResource resourceGroupResource = client.GetResourceGroupResource(resourceGroupResourceId);

            // get the collection of this PlaywrightTestingAccountResource
            PlaywrightTestingAccountCollection collection = resourceGroupResource.GetPlaywrightTestingAccounts();

            // invoke the operation
            string accountName = "saradadotnetTestAccount2";
            PlaywrightTestingAccountData data = new PlaywrightTestingAccountData(new AzureLocation("westus3"))
            {
                Properties = new PlaywrightTestingAccountProperties
                {
                    RegionalAffinity = PlaywrightTestingEnablementStatus.Disabled,
                },
                Tags =
                    {
                        ["Team"] = "Playwright Testing Service"
                    },
            };
            ArmOperation<PlaywrightTestingAccountResource> lro = await collection.CreateOrUpdateAsync(WaitUntil.Completed, accountName, data);

            PlaywrightTestingAccountResource result = lro.Value;

            // the variable result is a resource, you could call other operations on this instance as well
            // but just for demo, we get its data from this resource instance
            PlaywrightTestingAccountData resourceData = result.Data;
            // for demo we just print out the id
            Console.WriteLine($"Succeeded on id: {resourceData.Id}");
        }
        public static async Task AccountDelete()
        {
            try
            {
                TokenCredential cred = new DefaultAzureCredential();
                ArmClient client = new(cred);

                string subscriptionId = "9fd9eff4-f386-452e-9893-06417ff6e808";
                string resourceGroupName = "bugtest-rg";
                ResourceIdentifier resourceGroupResourceId = ResourceGroupResource.CreateResourceIdentifier(subscriptionId, resourceGroupName);
                ResourceGroupResource resourceGroupResource = client.GetResourceGroupResource(resourceGroupResourceId);

                PlaywrightTestingAccountCollection collection = resourceGroupResource.GetPlaywrightTestingAccounts();

                string accountName = "saradadotnetTestAccount";
                PlaywrightTestingAccountResource accountResource = collection.Get(accountName);
                var lro = await accountResource.DeleteAsync(WaitUntil.Completed);
                Console.WriteLine(lro);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }
}