﻿namespace UCEME.Utilidades.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass()]
    public class NotificacionesTests
    {
        private readonly string emailTo = "julio_cejudo@yahoo.com";

        [TestMethod()]
        public void SendPasswordRetrievalTest()
        {
            var sut = Utilidades.Notificaciones.SendPasswordRetrieval(this.emailTo, "AAA");
            Assert.IsFalse(sut);
        }

        [TestMethod()]
        public void SendNewUserCreatedTest()
        {
            var sut = Utilidades.Notificaciones.SendNewUserCreated(this.emailTo, "AAA");
            Assert.IsFalse(sut);
        }

        [TestMethod()]
        public void EnviarCorreoContactoTest()
        {
            var sut = Utilidades.Notificaciones.EnviarCorreoContacto(this.emailTo, "AAA", "BBB");
            Assert.IsFalse(sut);
        }

        [TestMethod()]
        public void ModificarCitasMedicosTest()
        {
            var sut = Utilidades.Notificaciones.ModificarCitasMedicos(new Uceme.Model.Models.Cita());
            Assert.IsFalse(sut);
        }

        [TestMethod()]
        public void NotificarCitasMedicosTest()
        {
            var sut = Utilidades.Notificaciones.NotificarCitasMedicos(new Uceme.Model.Models.Cita(), "AAA");
            Assert.IsFalse(sut);
        }

        [TestMethod()]
        public void NotificarCitasUsuarioTest()
        {
            var sut = Utilidades.Notificaciones.NotificarCitasUsuario(new Uceme.Model.Models.Cita(), "AAA");
            Assert.IsFalse(sut);
        }
    }
}