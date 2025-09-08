using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AppMovil.ViewModels
{
    public class BuscarLibrosViewModel : INotifyPropertyChanged
    {
        private string _searchText = string.Empty;
        private bool _isBusy;
        private readonly List<string> _todosLosLibros;

        public string SearchText
        {
            get => _searchText;
            set 
            { 
                _searchText = value; 
                OnPropertyChanged(); 
                ((Command)BuscarCommand).ChangeCanExecute();
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> Libros { get; set; } = new();
        public ICommand BuscarCommand { get; }
        public ICommand LimpiarCommand { get; }

        public BuscarLibrosViewModel()
        {
            // Simulación de datos iniciales
            _todosLosLibros = new List<string>
            {
                "Cien años de soledad - Gabriel García Márquez",
                "La casa de los espíritus - Isabel Allende", 
                "Rayuela - Julio Cortázar",
                "El amor en los tiempos del cólera - Gabriel García Márquez",
                "Ficciones - Jorge Luis Borges",
                "Pedro Páramo - Juan Rulfo",
                "La ciudad y los perros - Mario Vargas Llosa"
            };

            BuscarCommand = new Command(OnBuscar, CanBuscar);
            LimpiarCommand = new Command(OnLimpiar);

            // Cargar todos los libros inicialmente
            CargarTodosLosLibros();
        }

        private bool CanBuscar()
        {
            return !string.IsNullOrWhiteSpace(SearchText) && SearchText.Length >= 2;
        }

        private async void OnBuscar()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                await Task.Delay(500); // Simula búsqueda en API

                var resultados = _todosLosLibros
                    .Where(l => l.ToLower().Contains(SearchText.ToLower()))
                    .ToList();

                Libros.Clear();
                foreach (var libro in resultados)
                {
                    Libros.Add(libro);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnLimpiar()
        {
            SearchText = string.Empty;
            CargarTodosLosLibros();
        }

        private void CargarTodosLosLibros()
        {
            Libros.Clear();
            foreach (var libro in _todosLosLibros)
            {
                Libros.Add(libro);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
