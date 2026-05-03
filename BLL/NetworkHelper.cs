using System.Linq;
using System.Net.NetworkInformation;

namespace BLL
{
    public class NetworkHelper
    {
        /// <summary>
        /// Obtiene la dirección MAC física real de la tarjeta de red del equipo donde se ejecuta el programa.
        /// Aislado en este helper para permitir futuros mocks en entornos de prueba si fuera necesario.
        /// </summary>
        /// <returns>Dirección MAC en formato de string (ej. 001122334455), o null si no encuentra.</returns>
        public string GetPhysicalMacAddress()
        {
            // Busca la primera interfaz de red activa que no sea loopback o túnel virtual
            var networkInterface = NetworkInterface.GetAllNetworkInterfaces()
                .FirstOrDefault(nic => nic.OperationalStatus == OperationalStatus.Up && 
                                       nic.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                                       nic.NetworkInterfaceType != NetworkInterfaceType.Tunnel);

            if (networkInterface != null)
            {
                return networkInterface.GetPhysicalAddress().ToString();
            }

            return null;
        }
    }
}
