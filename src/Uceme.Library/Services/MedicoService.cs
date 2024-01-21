namespace Uceme.Library.Services;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Logging;
using Uceme.Model.Data;
using Uceme.Model.Models;

public class MedicoService : IMedicoService
{
    private readonly ILogger<MedicoService> logger;

    private readonly ApplicationDbContext context;

    public MedicoService(
        ILogger<MedicoService> logger,
        IApplicationDbContext context)
    {
        this.logger = logger;
        this.context = (ApplicationDbContext)context;
    }

    public IEnumerable<Usuario> GetMedicoMinVista(bool hackOrder)
    {
        try
        {
            IQueryable<Usuario> data = this.context.Usuario.Where(us => us.idRol == 2).OrderBy(o => o.display_order).Select(o => new Usuario
            {
                idUsuario = o.idUsuario,
                nombre = o.nombre,
                apellidos = o.apellidos,
                foto = o.foto,
            });

            this.logger.LogInformation("retrieved {Count} items", data.Count());

            return data;
        }
        catch (Exception e)
        {
            throw new DataException("Error retrieving MedicoMinVista", e);
        }
    }
}
