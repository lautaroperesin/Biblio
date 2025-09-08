using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AppMovil.ViewModels
{
    public class PrestamosViewModel : INotifyPropertyChanged
    {
        private bool _isBusy;
        private string _mensajeVacio = "No tienes préstamos activos";

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

        public bool TienePrestamos => Prestamos.Count > 0;

        public ObservableCollection<PrestamoItem> Prestamos { get; set; } = new();
        public ICommand RefrescarCommand { get; }
        public ICommand RenovarCommand { get; }

        public PrestamosViewModel()
        {
            RefrescarCommand = new Command(OnRefrescar);
            RenovarCommand = new Command<PrestamoItem>(OnRenovar);

            CargarPrestamos();
        }

        private async void OnRefrescar()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                await Task.Delay(1000); // Simula llamada a API
                CargarPrestamos();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnRenovar(PrestamoItem prestamo)
        {
            if (prestamo == null || IsBusy) return;

            try
            {
                IsBusy = true;
                await Task.Delay(500); // Simula renovación

                // Extender fecha de vencimiento por 2 semanas
                prestamo.FechaVencimiento = prestamo.FechaVencimiento.AddDays(14);
                OnPropertyChanged(nameof(Prestamos));
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void CargarPrestamos()
        {
            Prestamos.Clear();

            // Simulación de datos
            Prestamos.Add(new PrestamoItem
            {
                Id = 1,
                TituloLibro = "Cien años de soledad",
                Autor = "Gabriel García Márquez",
                FechaPrestamo = DateTime.Now.AddDays(-7),
                FechaVencimiento = DateTime.Now.AddDays(7),
                EstadoPrestamo = "Activo"
            });

            Prestamos.Add(new PrestamoItem
            {
                Id = 2,
                TituloLibro = "La casa de los espíritus",
                Autor = "Isabel Allende",
                FechaPrestamo = DateTime.Now.AddDays(-14),
                FechaVencimiento = DateTime.Now.AddDays(-1),
                EstadoPrestamo = "Vencido"
            });

            OnPropertyChanged(nameof(TienePrestamos));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class PrestamoItem : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string TituloLibro { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public DateTime FechaPrestamo { get; set; }

        private DateTime _fechaVencimiento;
        public DateTime FechaVencimiento
        {
            get => _fechaVencimiento;
            set { _fechaVencimiento = value; OnPropertyChanged(); OnPropertyChanged(nameof(DiasRestantes)); OnPropertyChanged(nameof(EstaVencido)); }
        }

        public string EstadoPrestamo { get; set; } = string.Empty;

        public int DiasRestantes => (FechaVencimiento - DateTime.Now).Days;
        public bool EstaVencido => DateTime.Now > FechaVencimiento;
        public string ColorEstado => EstaVencido ? "#F44336" : (DiasRestantes <= 3 ? "#FF9800" : "#4CAF50");

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
