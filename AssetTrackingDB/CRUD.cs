using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AssetTrackingDB
{
    internal class CRUD
    {
        Message message = new Message();

        // ** CREATE **
        public void AddAsset()
        {
            MyDbContext Context = new MyDbContext();

            Asset asset = new Asset();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(" Please enter the following information -->");
            Console.ResetColor();

            Console.Write(" Brand: ");
            string brand = Console.ReadLine();

            bool isBrandEmpty = string.IsNullOrWhiteSpace(brand);

            while (isBrandEmpty)
            {
                message.DisplayErrorMessage(" Please enter brand: ");
                brand = Console.ReadLine();
                isBrandEmpty = string.IsNullOrWhiteSpace(brand);
            }

            asset.Brand = brand;

            Console.Write(" Model: ");
            string model = Console.ReadLine();

            bool isModelEmpty = string.IsNullOrWhiteSpace(model);

            while (isModelEmpty)
            {
                message.DisplayErrorMessage(" Please enter model: ");
                model = Console.ReadLine();
                isModelEmpty = string.IsNullOrWhiteSpace(model);
            }

            asset.Model = model;

            Console.Write(" Purchase date (YY-MM-DD): ");
            string date = Console.ReadLine();

            DateTime purchaseDate;
            bool isDate = DateTime.TryParse(date, out purchaseDate);

            try //Try-catch if input is not DateTime type
            {
                purchaseDate = Convert.ToDateTime(date);
            }
            catch (FormatException)
            {
                while (!isDate)
                {
                    message.DisplayErrorMessage(" Please enter a valid date (YY-MM-DD): ");
                    date = Console.ReadLine();
                    isDate = DateTime.TryParse(date, out purchaseDate);
                }
            }

            while (purchaseDate > DateTime.Now)
            {
                message.DisplayErrorMessage(" Please enter a date earlier than today: ");
                date = Console.ReadLine();
                isDate = DateTime.TryParse(date, out purchaseDate);
            }

            asset.PurchaseDate = purchaseDate;

            Console.Write(" Price: ");
            string price = Console.ReadLine();

            bool isPriceInt = int.TryParse(price, out int priceInt);

            try
            {
                priceInt = Convert.ToInt32(price);
            }
            catch (FormatException)
            {
                while (!isPriceInt)
                {
                    message.DisplayErrorMessage(" Please enter a price: ");
                    price = Console.ReadLine();
                    isPriceInt = int.TryParse(price, out priceInt);
                }
            }

            asset.Price = priceInt;

            Console.Write(" Is the product a (1) laptop or (2) mobile phone? ");
            string type = (Console.ReadLine());

            bool isTypeInt = int.TryParse(type, out int typeInt);

            try
            {
                typeInt = Convert.ToInt32(type);
            }
            catch (FormatException)
            {
                while (!isTypeInt)
                {
                    message.DisplayErrorMessage(" Please enter (1) for laptop or (2) for mobile phone: ");
                    type = Console.ReadLine();
                    isTypeInt = int.TryParse(type, out typeInt);
                }
            }

            if (isTypeInt)
            {
                while (typeInt != 1 && typeInt != 2) //If input is not 1 or 2
                {
                    message.DisplayErrorMessage(" Please enter (1) for laptop or (2) for mobile phone: ");
                    type = Console.ReadLine();
                    isTypeInt = int.TryParse(type, out typeInt);
                }

                if (typeInt == 1) //Add product to asset list - laptop type
                {
                    asset.Type = "Laptop";

                }
                else if (typeInt == 2) //Add product to asset list - mobile phone type
                {
                    asset.Type = "MobilePhone";
                }
            }

            try
            {
                Context.Assets.Add(asset);
                Context.SaveChanges();
            }
            catch (Exception)
            {
                message.DisplayErrorMessage(" Was not able to save product to list.");
            }

            message.DisplaySuccessMessage($" Added {asset.Brand} {asset.Model} to list.");
        }


        // ** READ **

        //Display assets - different color depending on End of Life
        public void ShowList()
        {
            MyDbContext Context = new MyDbContext();

            var assets = Context.Assets.ToList();
            var sortedAssets = Context.Assets.OrderBy(x => x.Type).ThenBy(x => x.PurchaseDate);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-15} {4,-15} {5,-10}", " Id", "Type", "Brand", "Model", "Purchase Date", "Price");
            Console.WriteLine(new string('-', 75));
            Console.ResetColor();

            foreach (Asset asset in sortedAssets)
            {
                bool isEndOfLifeNear = IsEndOfLifeNear(asset.PurchaseDate);
                bool isEndOfLifeAlmostNear = IsEndOfLifeAlmostNear(asset.PurchaseDate);

                if (isEndOfLifeNear)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                else if (isEndOfLifeAlmostNear)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                }

                Console.WriteLine("{0,-5} {1,-15} {2,-15} {3,-15} {4,-15} {5,-10}", $" {asset.Id}", asset.Type, asset.Brand, asset.Model, asset.PurchaseDate.ToString("yy/MM/dd"), asset.Price);
                Console.ResetColor();

            }

        }

        //True if End of Life is closer than 3 months (or overdue)
        public bool IsEndOfLifeNear(DateTime purchaseDate)
        {
            DateTime endOfLifeDate = purchaseDate.AddYears(3);
            DateTime thresholdDate = DateTime.Now.AddMonths(3);

            return endOfLifeDate < thresholdDate;
        }

        //True if End of Life is closer than 6 months
        public bool IsEndOfLifeAlmostNear(DateTime purchaseDate)
        {
            DateTime endOfLifeDate = purchaseDate.AddYears(3);
            DateTime thresholdDate = DateTime.Now.AddMonths(6);

            return endOfLifeDate < thresholdDate;
        }

        //** UPDATE **

        public void EditAsset()
        {
            MyDbContext Context = new MyDbContext();

            ShowList();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(" Please enter the Id of the product you want to edit: ");
            Console.ResetColor();

            string editId = Console.ReadLine();

            bool isEditIdInt = int.TryParse(editId, out int editIdInt);

            try
            {
                editIdInt = Convert.ToInt32(editId);
            }
            catch (FormatException)
            {
                while (!isEditIdInt)
                {
                    message.DisplayErrorMessage(" Please enter the Id (number) of the product you want to edit: ");
                    editId = Console.ReadLine();
                    isEditIdInt = int.TryParse(editId, out editIdInt);
                }
            }

            while (!Context.Assets.Any(x => x.Id.Equals(editIdInt)))
            {
                message.DisplayErrorMessage(" There is no product with that Id, please enter a valid Id: ");
                editId = Console.ReadLine();
                isEditIdInt = int.TryParse(editId, out editIdInt);
            }

            Asset asset = Context.Assets.FirstOrDefault(x => x.Id.Equals(editIdInt));

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(" Choose what you want to edit -->");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine(" (1) Type");
            Console.WriteLine(" (2) Brand");
            Console.WriteLine(" (3) Model");
            Console.WriteLine(" (4) Purchase Date");
            Console.WriteLine(" (5) Price");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(" My choice: ");
            Console.ResetColor();

            string editField = Console.ReadLine();

            bool isEditFieldInt = int.TryParse(editField, out int editFieldInt);

            try
            {
                editFieldInt = Convert.ToInt32(editField);
            }
            catch (FormatException)
            {
                while (!isEditFieldInt)
                {
                    message.DisplayErrorMessage(" Please enter what you want to edit (number)? ");
                    editField = Console.ReadLine();
                    isEditFieldInt = int.TryParse(editField, out editFieldInt);
                }
            }

            while (isEditFieldInt && editFieldInt < 1 || editFieldInt > 5)
            {
                message.DisplayErrorMessage(" You can only choose (1), (2), (3), (4) or (5): ");
                editField = Console.ReadLine();
                isEditFieldInt = int.TryParse(editField, out editFieldInt);
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(" Make your update: ");
            Console.ResetColor();

            string newValue = Console.ReadLine();

            bool isNewValueInt = int.TryParse(newValue, out int newValueInt);
            bool isNewValueEmpty = string.IsNullOrWhiteSpace(newValue);

            while (isNewValueEmpty)
            {
                message.DisplayErrorMessage(" Please enter a value: ");
                newValue = Console.ReadLine();
                isNewValueEmpty = string.IsNullOrWhiteSpace(newValue);
            }

            switch (editFieldInt)
            {
                case 1:
                    Console.Write(" Is the product a (1) laptop or (2) mobile phone? ");
                    string type = (Console.ReadLine());

                    bool isTypeInt = int.TryParse(type, out int typeInt);

                    try
                    {
                        typeInt = Convert.ToInt32(type);
                    }
                    catch (FormatException)
                    {
                        while (!isTypeInt)
                        {
                            message.DisplayErrorMessage(" Please enter (1) for laptop or (2) for mobile phone: ");
                            type = Console.ReadLine();
                            isTypeInt = int.TryParse(type, out typeInt);
                        }
                    }

                    if (isTypeInt)
                    {
                        while (typeInt != 1 && typeInt != 2) //If input is not 1 or 2
                        {
                            message.DisplayErrorMessage(" Please enter (1) for laptop or (2) for mobile phone: ");
                            type = Console.ReadLine();
                            isTypeInt = int.TryParse(type, out typeInt);
                        }

                        if (typeInt == 1) //Add product to asset list - laptop type
                        {
                            asset.Type = "Laptop";

                        }
                        else if (typeInt == 2) //Add product to asset list - mobile phone type
                        {
                            asset.Type = "MobilePhone";
                        }
                    }
                    break;
                case 2:
                    asset.Brand = newValue;
                    break;
                case 3:
                    asset.Model = newValue;
                    break;
                case 4:
                    DateTime newValueDate;
                    bool isDate = DateTime.TryParse(newValue, out newValueDate);

                    try //Try-catch if input is not DateTime type
                    {
                        newValueDate = Convert.ToDateTime(newValue);
                    }
                    catch (FormatException)
                    {
                        while (!isDate)
                        {
                            message.DisplayErrorMessage(" Please enter a valid date (YY-MM-DD): ");
                            newValue = Console.ReadLine();
                            isDate = DateTime.TryParse(newValue, out newValueDate);
                        }
                    }

                    while (newValueDate > DateTime.Now)
                    {
                        message.DisplayErrorMessage(" Please enter a date earlier than today: ");
                        newValue = Console.ReadLine();
                        isDate = DateTime.TryParse(newValue, out newValueDate);
                    }

                    asset.PurchaseDate = newValueDate;
                    break;
                case 5:
                    try
                    {
                        newValueInt = Convert.ToInt32(newValue);
                    }
                    catch (FormatException)
                    {
                        while (!isNewValueInt)
                        {
                            message.DisplayErrorMessage(" Enter a price (number): ");
                            newValue = Console.ReadLine();
                            isNewValueInt = int.TryParse(newValue, out newValueInt);
                        }
                    }
                    asset.Price = newValueInt;
                    break;

            }

            try
            {
                Context.Assets.Update(asset);
                Context.SaveChanges();
            }
            catch (Exception)
            {
                message.DisplayErrorMessage(" Was not able to edit product.");
            }


            Asset editedAsset = Context.Assets.FirstOrDefault(x => x.Id.Equals(editIdInt));

            message.DisplaySuccessMessage($" Product with Id {editIdInt} is edited - {editedAsset.Brand} {editedAsset.Model}.");
        }

        public void DeleteAsset()
        {
            MyDbContext Context = new MyDbContext();

            ShowList();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(" Please enter the Id of the product you want to delete: ");
            Console.ResetColor();

            string deleteId = Console.ReadLine();

            bool isDeleteIdInt = int.TryParse(deleteId, out int deleteIdInt);

            try
            {
                deleteIdInt = Convert.ToInt32(deleteId);
            }
            catch (FormatException)
            {
                while (!isDeleteIdInt)
                {
                    message.DisplayErrorMessage(" Enter Id (number) of the product you want to edit: ");
                    deleteId = Console.ReadLine();
                    isDeleteIdInt = int.TryParse(deleteId, out deleteIdInt);
                }
            }

            while (!Context.Assets.Any(x => x.Id.Equals(deleteIdInt)))
            {
                message.DisplayErrorMessage(" There is no product with that Id, please enter a valid Id: ");
                deleteId = Console.ReadLine();
                isDeleteIdInt = int.TryParse(deleteId, out deleteIdInt);
            }

            Asset asset = Context.Assets.FirstOrDefault(x => x.Id.Equals(deleteIdInt));

            try
            {
                Context.Assets.Remove(asset);
                Context.SaveChanges();
            }
            catch (Exception)
            {
                message.DisplayErrorMessage(" Was not able to delete product from list.");
            }

            message.DisplaySuccessMessage($" Product with Id {deleteIdInt} is deleted - {asset.Brand} {asset.Model}.");
        }
    }
}
