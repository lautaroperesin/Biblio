using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Service.DTOs;
using Service.Models;
using Service.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppMovil.ViewModels
{
    public partial class BuscarLibrosViewModel : ObservableObject
    {
        LibroService _libroService = new();

        [ObservableProperty]
        private string searchText = string.Empty;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private ObservableCollection<Libro> libros = new();

        // Propiedades para los filtros
        [ObservableProperty]
        private bool filtrarPorTitulo = true;

        [ObservableProperty]
        private bool filtrarPorAutor = false;

        [ObservableProperty]
        private bool filtrarPorEditorial = false;

        [ObservableProperty]
        private bool filtrarPorGenero = false;

        [ObservableProperty]
        private bool mostrarFiltros = false;

        private List<Libro> _todosLosLibros = new();

        public IRelayCommand BuscarCommand { get; }
        public IRelayCommand LimpiarCommand { get; }
        public IRelayCommand ToggleFiltrosCommand { get; }

        public BuscarLibrosViewModel()
        {
            BuscarCommand = new RelayCommand(OnBuscar);
            LimpiarCommand = new RelayCommand(OnLimpiar);
            ToggleFiltrosCommand = new RelayCommand(OnToggleFiltros);
            _ = InicializarAsync();
        }

        private async Task InicializarAsync()
        {
            OnBuscar();
        }

        partial void OnSearchTextChanged(string value)
        {
            //if (string.IsNullOrEmpty(value)) OnBuscar();
        }

        // Los cambios en filtros también disparan nueva búsqueda
        //partial void OnFiltrarPorTituloChanged(bool value) => OnBuscar();
        //partial void OnFiltrarPorAutorChanged(bool value) => OnBuscar();
        //partial void OnFiltrarPorEditorialChanged(bool value) => OnBuscar();
        //partial void OnFiltrarPorGeneroChanged(bool value) => OnBuscar();

        private async void OnBuscar()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                FilterLibroDTO filtro = new()
                {
                    SearchText = this.SearchText,
                    ForTitulo = this.FiltrarPorTitulo,
                    ForAutor = this.FiltrarPorAutor,
                    ForEditorial = this.FiltrarPorEditorial,
                    ForGenero = this.FiltrarPorGenero
                };
                var librosFiltrados = await _libroService.GetWithFilterAsync(filtro);

                Libros = librosFiltrados != null ? new ObservableCollection<Libro>(librosFiltrados)
                    : new ObservableCollection<Libro>();

                //_libroService.GetWithFilterAsync(filtro);

                //Libros = new ObservableCollection<Libro>(librosFiltrados);
            }
            finally
            {
                IsBusy = false;
            }
        }


        private void OnLimpiar()
        {
            SearchText = string.Empty;
            // Mantener los filtros pero ejecutar búsqueda limpia
            OnBuscar();
        }

        private void OnToggleFiltros()
        {
            MostrarFiltros = !MostrarFiltros;
        }
    }
}
