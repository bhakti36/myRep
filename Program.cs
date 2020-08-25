using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
  public  class SKUwise_Price
    {
        public string SKU_Name { get; set; }

        public int SKU_Price { get; set; }
    }
   public class SKUwise_Promotion
    {
        public string SKU_Nmae { get; set; }

        public int SKU_discount_Price { get; set; }
        public int SKU_Count { get; set; }
    }

    

   public interface IPromotion_SKU_Unit
    {
        int Calculate_SKU_Unit();

    }
   public class SKU_Club_Promotion : IPromotion_SKU_Unit
    {
      
        int SKU_C_Unit, SKU_D_Unit;
        
        List<SKUwise_Price> sku_objlist;
        SKUwise_Promotion SKU_Pro_obj;

        public SKU_Club_Promotion(int SKU_C_Unit,int SKU_D_Unit, List<SKUwise_Price> SKU_List, SKUwise_Promotion SKU_Pro_obj)
        {
            this.SKU_C_Unit = SKU_C_Unit;
            this.SKU_D_Unit = SKU_D_Unit;
            this.sku_objlist = SKU_List;
            this.SKU_Pro_obj = SKU_Pro_obj;
        }
        public int Calculate_SKU_Unit()
        {
            string[] skuname = SKU_Pro_obj.SKU_Nmae.Split(',');
            var skuc_price = sku_objlist.Where(m => m.SKU_Name.Contains(skuname[0])).FirstOrDefault(); 
            var skud_price = sku_objlist.Where(m => m.SKU_Name.Contains(skuname[1])).FirstOrDefault();

            if (SKU_C_Unit == SKU_D_Unit)
                return SKU_C_Unit * SKU_Pro_obj.SKU_discount_Price;
            else if (SKU_C_Unit < SKU_D_Unit)
            {

                return SKU_C_Unit * SKU_Pro_obj.SKU_discount_Price + ((SKU_D_Unit- SKU_C_Unit) * skud_price.SKU_Price);
            }
            else {
                return SKU_D_Unit * SKU_Pro_obj.SKU_discount_Price + ((SKU_C_Unit - SKU_D_Unit) * skuc_price.SKU_Price);
            }
        }
    }


  public  class SingleSKU_Promotion: IPromotion_SKU_Unit
    {
        int SKU_Unit;
        Dictionary<string, int> SKU_Price;
       
        List<SKUwise_Price> sku_objlist;
        SKUwise_Promotion SKU_Pro_obj;
        public SingleSKU_Promotion(int SKU_Unit, List<SKUwise_Price> SKU_List, SKUwise_Promotion SKU_Pro_obj)
        {
            this.SKU_Unit = SKU_Unit;
           
            this.sku_objlist = SKU_List;
            this.SKU_Pro_obj = SKU_Pro_obj;
        }
        public  int Calculate_SKU_Unit()
        {
            int div = SKU_Unit / SKU_Pro_obj.SKU_Count; //quotient is 1
            int mod = SKU_Unit % SKU_Pro_obj.SKU_Count; //remainder is 2
            var skua_price = sku_objlist.Where(m => m.SKU_Name == SKU_Pro_obj.SKU_Nmae).FirstOrDefault();


            return (div * SKU_Pro_obj.SKU_discount_Price) + (mod* skua_price.SKU_Price);
        }
    }

   public  class ClsPromotion
    {
        //  public double billAmt { get; set; }

        ConsoleApplication3.IPromotion_SKU_Unit currentStrategy;
        public ClsPromotion(IPromotion_SKU_Unit newStrategy)
        {
            currentStrategy = newStrategy;
        }

        public int Calculate_SKU_Unit()
        {
            return currentStrategy.Calculate_SKU_Unit();
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            List<SKUwise_Price> sku_obj = new List<ConsoleApplication3.SKUwise_Price>();
            sku_obj.Add(new SKUwise_Price() { SKU_Name = "A", SKU_Price = 50 });
            sku_obj.Add(new SKUwise_Price() { SKU_Name = "B", SKU_Price = 30 });
            sku_obj.Add(new SKUwise_Price() { SKU_Name = "C", SKU_Price = 20 });
            sku_obj.Add(new SKUwise_Price() { SKU_Name = "D", SKU_Price = 15 });

            SKUwise_Promotion SKUA_Pro_obj = new SKUwise_Promotion();
            SKUA_Pro_obj.SKU_Count = 3;
            SKUA_Pro_obj.SKU_Nmae = "A";
            SKUA_Pro_obj.SKU_discount_Price = 130;

            SKUwise_Promotion SKUB_Pro_obj = new SKUwise_Promotion();
            SKUB_Pro_obj.SKU_Count = 2;
            SKUB_Pro_obj.SKU_Nmae = "B";
            SKUB_Pro_obj.SKU_discount_Price = 45;

            SKUwise_Promotion SKUclub_Pro_obj = new SKUwise_Promotion();
            SKUclub_Pro_obj.SKU_Count = 1;
            SKUclub_Pro_obj.SKU_Nmae = "C,D";
            SKUclub_Pro_obj.SKU_discount_Price = 30;


            int SKU_A_Unit;
            int SKU_B_Unit;
            int SKU_C_Unit;
            int SKU_D_Unit;
            int total;

            Console.WriteLine("enter Unit for SKU A: ");
            SKU_A_Unit = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("enter Unit for SKU B: ");
            SKU_B_Unit = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("enter Unit for SKU C: ");
            SKU_C_Unit = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("enter Unit for SKU D: ");
            SKU_D_Unit = Convert.ToInt32(Console.ReadLine());


           
            ClsPromotion skuA = new ClsPromotion(new SingleSKU_Promotion(SKU_A_Unit, sku_obj, SKUA_Pro_obj));
            int SKUA_Total=skuA.Calculate_SKU_Unit();
            ClsPromotion skuB = new ClsPromotion(new SingleSKU_Promotion(SKU_B_Unit, sku_obj, SKUB_Pro_obj));
            int SKUB_Total = skuB.Calculate_SKU_Unit();
            ClsPromotion sku_club = new ClsPromotion(new SKU_Club_Promotion(SKU_C_Unit, SKU_D_Unit, sku_obj, SKUclub_Pro_obj));
            int club_Total = sku_club.Calculate_SKU_Unit();


            total = SKUA_Total + SKUB_Total + club_Total;

            Console.WriteLine("---------------------------------------------------- ");
            Console.WriteLine("total: " + total);
            Console.ReadLine();

        }
    }


}
