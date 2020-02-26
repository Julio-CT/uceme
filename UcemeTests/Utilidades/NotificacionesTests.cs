namespace UCEME.Utilidades.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using UCEME.Utilidades;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass()]
    public class NotificacionesTests
    {
        private readonly string emailTo = "julio_cejudo@yahoo.com";

        [TestMethod()]
        public void SendPasswordRetrievalTest()
        {
            Utilidades.Notificaciones.SendPasswordRetrieval(emailTo, "AAA");
            Assert.Fail();
        }

        [TestMethod()]
        public void SendNewUserCreatedTest()
        {
            Utilidades.Notificaciones.SendNewUserCreated(emailTo, "AAA");
            Assert.Fail();
        }

        [TestMethod()]
        public void EnviarCorreoContactoTest()
        {
            Utilidades.Notificaciones.EnviarCorreoContacto(emailTo, "AAA", "BBB");
            Assert.Fail();
        }

        [TestMethod()]
        public void ModificarCitasMedicosTest()
        {
            Utilidades.Notificaciones.ModificarCitasMedicos(new Models.Cita());
            Assert.Fail();
        }

        [TestMethod()]
        public void NotificarCitasMedicosTest()
        {
            Utilidades.Notificaciones.NotificarCitasMedicos(new Models.Cita(), "AAA");
            Assert.Fail();
        }

        [TestMethod()]
        public void NotificarCitasUsuarioTest()
        {
            Utilidades.Notificaciones.NotificarCitasUsuario(new Models.Cita(), "AAA");
            Assert.Fail();
        }
    }
}