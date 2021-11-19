using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace LosManantialesDelSurBackend.Models
{
    public partial class DBManantialesContext : DbContext
    {
        public DBManantialesContext()
        {
        }

        public DBManantialesContext(DbContextOptions<DBManantialesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<CodigoValidacionPago> CodigoValidacionPago { get; set; }
        public virtual DbSet<Destinatario> Destinatario { get; set; }
        public virtual DbSet<EstadoSeguimiento> EstadoSeguimiento { get; set; }
        public virtual DbSet<Paquete> Paquete { get; set; }
        public virtual DbSet<Pasaje> Pasaje { get; set; }
        public virtual DbSet<PrecioAsiento> PrecioAsiento { get; set; }
        public virtual DbSet<PrecioDistancia> PrecioDistancia { get; set; }
        public virtual DbSet<Seguimiento> Seguimiento { get; set; }
        public virtual DbSet<Sucursal> Sucursal { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Vehiculo> Vehiculo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=connectionDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Uuid)
                    .HasName("PK__Cliente__7F427930BAB10624");

                entity.HasIndex(e => e.Correo, "UQ__Cliente__2A586E0B0EBE0018")
                    .IsUnique();

                entity.HasIndex(e => e.Celular, "UQ__Cliente__2E4973E707DDFC6E")
                    .IsUnique();

                entity.HasIndex(e => e.Dni, "UQ__Cliente__D87608A728B580FB")
                    .IsUnique();

                entity.Property(e => e.Uuid)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("uuid");

                entity.Property(e => e.Celular)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("celular")
                    .IsFixedLength(true);

                entity.Property(e => e.Correo)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("correo");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Dni)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("dni")
                    .IsFixedLength(true);

                entity.Property(e => e.Nombres)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("nombres");
            });

            modelBuilder.Entity<CodigoValidacionPago>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Codigo)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("codigo")
                    .IsFixedLength(true);

                entity.Property(e => e.Usado).HasColumnName("usado");
            });

            modelBuilder.Entity<Destinatario>(entity =>
            {
                entity.HasKey(e => e.Uuid)
                    .HasName("PK__Destinat__7F42793049685E6F");

                entity.HasIndex(e => e.Celular, "UQ__Destinat__2E4973E7F2D94FD9")
                    .IsUnique();

                entity.Property(e => e.Uuid)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("uuid");

                entity.Property(e => e.Celular)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("celular")
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Dni)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("dni")
                    .IsFixedLength(true);

                entity.Property(e => e.Nombres)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("nombres");
            });

            modelBuilder.Entity<EstadoSeguimiento>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");

                entity.Property(e => e.Seguimiento)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("seguimiento");

                entity.HasOne(d => d.SeguimientoNavigation)
                    .WithMany(p => p.EstadoSeguimiento)
                    .HasForeignKey(d => d.Seguimiento)
                    .HasConstraintName("fk_Seguimiento");
            });

            modelBuilder.Entity<Paquete>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CodigoValidacion)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("codigo_validacion")
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Destinatario)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("destinatario");

                entity.Property(e => e.DestinoPaquete).HasColumnName("destino_paquete");

                entity.Property(e => e.Dimensiones)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("dimensiones");

                entity.Property(e => e.NumeroGuia)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("numero_guia")
                    .IsFixedLength(true);

                entity.Property(e => e.OrigenPaquete).HasColumnName("origen_paquete");

                entity.Property(e => e.Remitente)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("remitente");

                entity.Property(e => e.Seguimiento)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("seguimiento");

                entity.Property(e => e.Vehiculo).HasColumnName("vehiculo");

                entity.HasOne(d => d.DestinatarioNavigation)
                    .WithMany(p => p.Paquete)
                    .HasForeignKey(d => d.Destinatario)
                    .HasConstraintName("fk_Destinatario");

                entity.HasOne(d => d.DestinoPaqueteNavigation)
                    .WithMany(p => p.PaqueteDestinoPaqueteNavigation)
                    .HasForeignKey(d => d.DestinoPaquete)
                    .HasConstraintName("fk_DestinoPaquete");

                entity.HasOne(d => d.OrigenPaqueteNavigation)
                    .WithMany(p => p.PaqueteOrigenPaqueteNavigation)
                    .HasForeignKey(d => d.OrigenPaquete)
                    .HasConstraintName("fk_OrigenPaquete");

                entity.HasOne(d => d.RemitenteNavigation)
                    .WithMany(p => p.Paquete)
                    .HasForeignKey(d => d.Remitente)
                    .HasConstraintName("fk_Remitente");

                entity.HasOne(d => d.SeguimientoNavigation)
                    .WithMany(p => p.Paquete)
                    .HasForeignKey(d => d.Seguimiento)
                    .HasConstraintName("fk_SeguimientoPaquete");

                entity.HasOne(d => d.VehiculoNavigation)
                    .WithMany(p => p.Paquete)
                    .HasForeignKey(d => d.Vehiculo)
                    .HasConstraintName("fk_Vehiculo");
            });

            modelBuilder.Entity<Pasaje>(entity =>
            {
                entity.HasKey(e => e.Uuid)
                    .HasName("PK__Pasaje__7F427930B31CFAAE");

                entity.Property(e => e.Uuid)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("uuid");

                entity.Property(e => e.CodigoValidacion)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("codigo_validacion")
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.DestinoSucursal).HasColumnName("destino_sucursal");

                entity.Property(e => e.FechaLlegada)
                    .HasColumnType("date")
                    .HasColumnName("fecha_llegada");

                entity.Property(e => e.FechaSalida)
                    .HasColumnType("date")
                    .HasColumnName("fecha_salida");

                entity.Property(e => e.NumeroAsiento).HasColumnName("numero_asiento");

                entity.Property(e => e.OrigenSucursal).HasColumnName("origen_sucursal");

                entity.Property(e => e.Pasajero)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("pasajero");

                entity.Property(e => e.VehiculoPasaje).HasColumnName("vehiculo_pasaje");

                entity.HasOne(d => d.DestinoSucursalNavigation)
                    .WithMany(p => p.PasajeDestinoSucursalNavigation)
                    .HasForeignKey(d => d.DestinoSucursal)
                    .HasConstraintName("fk_DestinoSucursal");

                entity.HasOne(d => d.OrigenSucursalNavigation)
                    .WithMany(p => p.PasajeOrigenSucursalNavigation)
                    .HasForeignKey(d => d.OrigenSucursal)
                    .HasConstraintName("fk_OrigenSucursal");

                entity.HasOne(d => d.PasajeroNavigation)
                    .WithMany(p => p.Pasaje)
                    .HasForeignKey(d => d.Pasajero)
                    .HasConstraintName("fk_Pasajero");

                entity.HasOne(d => d.VehiculoPasajeNavigation)
                    .WithMany(p => p.Pasaje)
                    .HasForeignKey(d => d.VehiculoPasaje)
                    .HasConstraintName("fk_VehiculoPasaje");
            });

            modelBuilder.Entity<PrecioAsiento>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NumeroAsiento).HasColumnName("numero_asiento");

                entity.Property(e => e.PorcentajeVariacion).HasColumnName("porcentaje_variacion");
            });

            modelBuilder.Entity<PrecioDistancia>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DestinoSucursal)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("destino_sucursal");

                entity.Property(e => e.OrigenSucursal)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("origen_sucursal");

                entity.Property(e => e.PrecioPaquete).HasColumnName("precio_paquete");

                entity.Property(e => e.PrecioPasaje).HasColumnName("precio_pasaje");
            });

            modelBuilder.Entity<Seguimiento>(entity =>
            {
                entity.HasKey(e => e.Uuid)
                    .HasName("PK__Seguimie__7F4279306D6EA001");

                entity.Property(e => e.Uuid)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("uuid");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.FechEnvio)
                    .HasColumnType("datetime")
                    .HasColumnName("fech_envio");

                entity.Property(e => e.FechaEntrega)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_entrega");

                entity.Property(e => e.NumeroSeguimiento)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("numero_seguimiento");
            });

            modelBuilder.Entity<Sucursal>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CodigoPostal)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("codigo_postal")
                    .IsFixedLength(true);

                entity.Property(e => e.Distrito)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("distrito");

                entity.Property(e => e.Provincia)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("provincia");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Uuid)
                    .HasName("PK__Usuario__7F42793049DF5664");

                entity.HasIndex(e => e.Correo, "UQ__Usuario__2A586E0BDCBF5666")
                    .IsUnique();

                entity.HasIndex(e => e.Celular, "UQ__Usuario__2E4973E74D4C89D3")
                    .IsUnique();

                entity.HasIndex(e => e.Dni, "UQ__Usuario__D87608A76FBB21DC")
                    .IsUnique();

                entity.Property(e => e.Uuid)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("uuid");

                entity.Property(e => e.Celular)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("celular")
                    .IsFixedLength(true);

                entity.Property(e => e.Correo)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("correo");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("direccion");

                entity.Property(e => e.Dni)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("dni")
                    .IsFixedLength(true);

                entity.Property(e => e.Nombres)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("nombres");

                entity.Property(e => e.Pass)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("pass");

                entity.Property(e => e.Rol)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("rol");
            });

            modelBuilder.Entity<Vehiculo>(entity =>
            {
                entity.HasIndex(e => e.Placa, "UQ__Vehiculo__0C0574251FFC113B")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Conductor)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("conductor");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.Placa)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("placa")
                    .IsFixedLength(true);

                entity.Property(e => e.SucursalActual).HasColumnName("sucursal_actual");

                entity.HasOne(d => d.ConductorNavigation)
                    .WithMany(p => p.Vehiculo)
                    .HasForeignKey(d => d.Conductor)
                    .HasConstraintName("fk_Conductor");

                entity.HasOne(d => d.SucursalActualNavigation)
                    .WithMany(p => p.Vehiculo)
                    .HasForeignKey(d => d.SucursalActual)
                    .HasConstraintName("fk_SucursalActual");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
