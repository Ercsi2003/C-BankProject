
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BankProject
{
    internal class BankTest
    {
        Bank b;

        [SetUp]

        public void Setup()
        {
            b = new Bank();
            b.UjSzamla("Gipsz Jakab", "1234");
        }
        

        [Test]
        public void UjSzamlaEgyenleg0()
        {
           
            Assert.That(b.Egyenleg("1234"), Is.EqualTo(0));
        }

        [Test]
        public void UjSzamlaNullNev()
        {
           
            Assert.Throws<ArgumentNullException>(() => b.UjSzamla(null, "1234"));
        }

        [Test]
        public void UjSzamlaNullSzamlaszam()
        {
            
            Assert.Throws<ArgumentNullException>(() => b.UjSzamla("Teszt Elek", null));
        }



        [Test]
        public void UjSzamlaUresNev()
        {
            
            Assert.Throws<ArgumentException>(() => b.UjSzamla("", "1234"));
        }

        [Test]
        public void UjSzamlaUresSzamlaszam()
        {
            
            Assert.Throws<ArgumentException>(() => b.UjSzamla("Teszt Elek", ""));
        }

        [Test]
        public void UjSzamlaLetezoSzamlaszammal()
        {
            
           
            Assert.Throws<ArgumentException>(() => b.UjSzamla("Teszt Elek", "1234"));
        }

        [Test]
        public void UjSzamlaLetezoNevvel()
        {
            
           
            Assert.DoesNotThrow(() => b.UjSzamla("Gipsz Jakab", "4321"));
        }

        [Test]
        public void Egyenleg_NullSzamlaszam()
        {
            Assert.Throws<ArgumentNullException>(() => b.Egyenleg(null));
        }

        [Test]
        public void Egyenleg_UresSzamlaszam()
        {
            Assert.Throws<ArgumentException>(() => b.Egyenleg(""));
        }

        [Test]
        public void Egyenleg_NemLetezoSzamlaszam()
        {
            Assert.Throws<HibasSzamlaszamException>(() => b.Egyenleg("4321"));
        }

        [Test]
        public void EgyenlegFeltolt_NullSzamlaszam()
        {
            Assert.Throws<ArgumentNullException>(() => b.EgyenlegFeltolt(null, 10000));
        }

        [Test]
        public void EgyenlegFeltolt_UresSzamlaszam()
        {
            Assert.Throws<ArgumentException>(() => b.EgyenlegFeltolt("",10000));
        }

        [Test]
        public void EgyenlegFeltolt_NemLetezoSzamlaszam()
        {
            Assert.Throws<HibasSzamlaszamException>(() => b.EgyenlegFeltolt("4321",10000));
        }

        [Test]
        public void EgyenlegFeltolt_0Osszeg()
        {
            Assert.Throws<ArgumentException>(() => b.EgyenlegFeltolt("1234", 0));
        }

        [Test]
        public void EgyenlegFeltolt_OsszegMegvaltozik()
        {
            b.EgyenlegFeltolt("1234", 10000);
            Assert.That(b.Egyenleg("1234"), Is.EqualTo(10000));
        }

        [Test]
        public void EgyenlegFeltolt_OsszegHozzaadodik()
        {
            b.EgyenlegFeltolt("1234", 10000);
            Assert.That(b.Egyenleg("1234"), Is.EqualTo(10000));

            b.EgyenlegFeltolt("1234", 20000);
            Assert.That(b.Egyenleg("1234"), Is.EqualTo(30000));
        }

        [Test]
        public void EgyenlegFeltolt_JoSzamlaraKerulAzOsszeg()
        {
            b.UjSzamla("Teszt Elek", "4321");
            b.UjSzamla("Gipsz Jakab", "5678");

            b.EgyenlegFeltolt("1234", 10000);
            Assert.That(b.Egyenleg("1234"), Is.EqualTo(10000));

            b.EgyenlegFeltolt("4321", 20000);
            Assert.That(b.Egyenleg("4321"), Is.EqualTo(20000));

            Assert.That(b.Egyenleg("5678"), Is.Zero);
        }

        [Test]

        public void Utal_Setup()
        {
            b.EgyenlegFeltolt("1234", 20000);
            b.UjSzamla("Teszt Elek", "5678");
            b.EgyenlegFeltolt("5678", 10000);
        }

        [Test]

        public void Utal_0ForintotUtal()
        {
            Utal_Setup();
            Assert.Throws<ArgumentException>(() => b.Utal("1234", "5678", 0));
        }

        [Test]

        public void Utal_HonnanSzamlaszamNull()
        {
            Utal_Setup();
            Assert.Throws<ArgumentNullException>(() => b.Utal(null,"5678", 10000));
        }

        [Test]

        public void Utal_HovaSzamlaszamNull()
        {
            Utal_Setup();
            Assert.Throws<ArgumentNullException>(() => b.Utal("1234", null, 10000));
        }

        [Test]

        public void Utal_HonnanSzamlaszamUres()
        {
            Utal_Setup();
            Assert.Throws<ArgumentException>(() => b.Utal("", "5678", 10000));
        }

        [Test]

        public void Utal_HovaSzamlaszamUres()
        {
            Utal_Setup();
            Assert.Throws<ArgumentException>(() => b.Utal("1234","", 10000));
        }

        [Test]

        public void Utal_NemLetezikHonnanSzamla()
        {
            Utal_Setup();
            Assert.Throws<HibasSzamlaszamException>(() => b.Utal("3761", "5678", 20000));
        }

        [Test]

        public void Utal_NemLetezikHovaSzamla()
        {
            Utal_Setup();
            Assert.Throws<HibasSzamlaszamException>(() => b.Utal("1234", "9125", 20000));

        }

        [Test]

        public void Utal_VanEElegPenzAHonnanSzamlan()
        {
            Utal_Setup();
            Assert.True(b.Utal("1234", "5678", 10000));

            Assert.That(b.Egyenleg("1234"), Is.EqualTo(10000));
            Assert.That(b.Egyenleg("5678"), Is.EqualTo(20000));

        }

        [Test]

        public void Utal_TobbetAkarUtalniMintAmennyiPenzeVan()
        {
            Utal_Setup();
            Assert.False(b.Utal("1234", "5678", 20001));

            Assert.That(b.Egyenleg("1234"), Is.EqualTo(20000));
            Assert.That(b.Egyenleg("5678"), Is.EqualTo(10000));
        }

        [Test]

        public void Utal_PontannyitUtalMintAmennyiPenzeVan()
        {
            Utal_Setup();
            Assert.True(b.Utal("1234", "5678", 20000));

            Assert.That(b.Egyenleg("1234"), Is.EqualTo(0));
            Assert.That(b.Egyenleg("5678"), Is.EqualTo(30000));
        }

        [Test]

        public void Utal_NemUgyanazEAHonnanEsAHovaSzamlaszam()
        {
            Utal_Setup();
            Assert.Throws<ArgumentException>(() => b.Utal("1234", "1234", 10000));
        }


    }
}
