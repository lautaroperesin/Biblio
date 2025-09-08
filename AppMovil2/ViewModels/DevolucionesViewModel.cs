using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AppMovil.ViewModels
{
    public class DevolucionesViewModel : INotifyPropertyChanged
    {
        private bool _isBusy;
        private string _mensajeVacio = "No hay devoluciones registradas";

        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public string MensajeVacio
        {
            get => _mensajeVacio;
            set { _mensajeVacio = value; OnPropertyChanged(); }
        }

        public bool TieneDevoluciones => Devoluciones.Count > 0;

        public ObservableCollection<DevolucionItem> Devoluciones { get; set; } = new();
        public ICommand RefrescarCommand { get; }
        public ICommand VerDetalleCommand { get; }

        public DevolucionesViewModel()
        {
            RefrescarCommand = new Command(OnRefrescar);
            VerDetalleCommand = new Command<DevolucionItem>(OnVerDetalle);

            CargarDevoluciones();
        }

        private async void OnRefrescar()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                await Task.Delay(1000); // Simula llamada a API
                CargarDevoluciones();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnVerDetalle(DevolucionItem devolucion)
        {
            if (devolucion == null) return;

            // Aquí podrías navegar a una página de detalle
            await Application.Current.MainPage.DisplayAlert(
                "Detalle de Devolución", 
                $"Libro: {devolucion.TituloLibro}\nFecha devuelto: {devolucion.FechaDevolucion:dd/MM/yyyy}\nEstado: {devolucion.EstadoDevolucion}", 
                "OK");
        }

        private void CargarDevoluciones()
        {
            Devoluciones.Clear();
            
            // Simulación de datos
            Devoluciones.Add(new DevolucionItem
            {
                Id = 1,
                TituloLibro = "El amor en los tiempos del cólera",
                Autor = "Gabriel García Márquez",
                FechaPrestamo = DateTime.Now.AddDays(-30),
                FechaDevolucion = DateTime.Now.AddDays(-3),
                EstadoDevolucion = "Devuelto a tiempo",
                TuvoMulta = false
            });

            Devoluciones.Add(new DevolucionItem
            {
                Id = 2,
                TituloLibro = "Ficciones",
                Autor = "Jorge Luis Borges",
                FechaPrestamo = DateTime.Now.AddDays(-45),
                FechaDevolucion = DateTime.Now.AddDays(-10),
                EstadoDevolucion = "Devuelto con retraso",
                TuvoMulta = true,
                MontoMulta = 5.50m
            });

            Devoluciones.Add(new DevolucionItem
            {
                Id = 3,
                TituloLibro = "Pedro Páramo",
                Autor = "Juan Rulfo",
                FechaPrestamo = DateTime.Now.AddDays(-21),
                FechaDevolucion = DateTime.Now.AddDays(-1),
                EstadoDevolucion = "Devuelto a tiempo",
                TuvoMulta = false
            });

            OnPropertyChanged(nameof(TieneDevoluciones));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DevolucionItem
    {
        public int Id { get; set; }
        public string TituloLibro { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public string EstadoDevolucion { get; set; } = string.Empty;
        public bool TuvoMulta { get; set; }
        public decimal MontoMulta { get; set; }
        
        public string ColorEstado => TuvoMulta ? "#F44336" : "#4CAF50";
        public string TextoMulta => TuvoMulta ? $"Multa: ${MontoMulta:F2}" : "Sin multa";
        public int DiasTranscurridos => (DateTime.Now - FechaDevolucion).Days;
    }
}
