using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Models;

namespace Service.Interfaces
{
    public interface IGeminiService
    {
        Task<string?> GetPromptResponse(string textPrompt);
        Task<Libro?> GetLibroFromPortada(string imageUrl);
    }
}
