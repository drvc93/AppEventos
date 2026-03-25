import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { eventApi, type CreateZoneRequest } from '../services/api';

interface ZoneForm extends CreateZoneRequest {
  id: number;
}

export default function CreateEvent() {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const [name, setName] = useState('');
  const [date, setDate] = useState('');
  const [location, setLocation] = useState('');
  const [zones, setZones] = useState<ZoneForm[]>([
    { id: Date.now(), name: '', price: 0, capacity: 1 },
  ]);

  const addZone = () => {
    setZones([...zones, { id: Date.now(), name: '', price: 0, capacity: 1 }]);
  };

  const removeZone = (id: number) => {
    if (zones.length > 1) {
      setZones(zones.filter((z) => z.id !== id));
    }
  };

  const updateZone = (id: number, field: keyof CreateZoneRequest, value: string | number) => {
    setZones(zones.map((z) => (z.id === id ? { ...z, [field]: value } : z)));
  };

  const validate = (): string | null => {
    if (!name.trim()) return 'El nombre del evento es obligatorio.';
    if (!date) return 'La fecha del evento es obligatoria.';
    if (!location.trim()) return 'El lugar del evento es obligatorio.';
    for (const zone of zones) {
      if (!zone.name.trim()) return 'Cada zona debe tener un nombre.';
      if (zone.price < 0) return 'El precio debe ser mayor o igual a 0.';
      if (zone.capacity <= 0) return 'La capacidad debe ser mayor a 0.';
    }
    return null;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    const validationError = validate();
    if (validationError) {
      setError(validationError);
      return;
    }

    setLoading(true);
    try {
      await eventApi.createEvent({
        name,
        date: new Date(date).toISOString(),
        location,
        zones: zones.map(({ name, price, capacity }) => ({ name, price: Number(price), capacity: Number(capacity) })),
      });
      navigate('/');
    } catch (err: unknown) {
      if (err && typeof err === 'object' && 'response' in err) {
        const axiosErr = err as { response?: { data?: { errors?: Array<{ errorMessage: string }> } } };
        const serverErrors = axiosErr.response?.data?.errors;
        if (serverErrors && Array.isArray(serverErrors)) {
          setError(serverErrors.map((e) => e.errorMessage).join(', '));
        } else {
          setError('Error al crear el evento. Intente nuevamente.');
        }
      } else {
        setError('Error de conexión con el servidor.');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="max-w-2xl mx-auto p-6">
      <h1 className="text-3xl font-bold mb-6 text-gray-800">Registrar Evento</h1>

      {error && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
          {error}
        </div>
      )}

      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Nombre del evento *
          </label>
          <input
            type="text"
            value={name}
            onChange={(e) => setName(e.target.value)}
            className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="Ej: Concierto de Rock 2026"
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Fecha *
          </label>
          <input
            type="datetime-local"
            value={date}
            onChange={(e) => setDate(e.target.value)}
            className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Lugar *
          </label>
          <input
            type="text"
            value={location}
            onChange={(e) => setLocation(e.target.value)}
            className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="Ej: Arena CDMX"
          />
        </div>

        <div className="border-t pt-4 mt-4">
          <div className="flex justify-between items-center mb-3">
            <h2 className="text-lg font-semibold text-gray-700">Zonas</h2>
            <button
              type="button"
              onClick={addZone}
              className="bg-green-500 text-white px-3 py-1 rounded-lg text-sm hover:bg-green-600 transition"
            >
              + Agregar zona
            </button>
          </div>

          {zones.map((zone, idx) => (
            <div key={zone.id} className="bg-gray-50 p-4 rounded-lg mb-3 border">
              <div className="flex justify-between items-center mb-2">
                <span className="text-sm font-medium text-gray-600">Zona {idx + 1}</span>
                {zones.length > 1 && (
                  <button
                    type="button"
                    onClick={() => removeZone(zone.id)}
                    className="text-red-500 text-sm hover:text-red-700"
                  >
                    Eliminar
                  </button>
                )}
              </div>
              <div className="grid grid-cols-3 gap-3">
                <div>
                  <label className="block text-xs text-gray-500 mb-1">Nombre *</label>
                  <input
                    type="text"
                    value={zone.name}
                    onChange={(e) => updateZone(zone.id, 'name', e.target.value)}
                    className="w-full border border-gray-300 rounded px-2 py-1 text-sm focus:outline-none focus:ring-1 focus:ring-blue-500"
                    placeholder="Ej: VIP"
                  />
                </div>
                <div>
                  <label className="block text-xs text-gray-500 mb-1">Precio *</label>
                  <input
                    type="number"
                    min="0"
                    step="0.01"
                    value={zone.price}
                    onChange={(e) => updateZone(zone.id, 'price', parseFloat(e.target.value) || 0)}
                    className="w-full border border-gray-300 rounded px-2 py-1 text-sm focus:outline-none focus:ring-1 focus:ring-blue-500"
                  />
                </div>
                <div>
                  <label className="block text-xs text-gray-500 mb-1">Capacidad *</label>
                  <input
                    type="number"
                    min="1"
                    value={zone.capacity}
                    onChange={(e) => updateZone(zone.id, 'capacity', parseInt(e.target.value) || 1)}
                    className="w-full border border-gray-300 rounded px-2 py-1 text-sm focus:outline-none focus:ring-1 focus:ring-blue-500"
                  />
                </div>
              </div>
            </div>
          ))}
        </div>

        <button
          type="submit"
          disabled={loading}
          className="w-full bg-blue-600 text-white py-3 rounded-lg font-semibold hover:bg-blue-700 transition disabled:opacity-50 disabled:cursor-not-allowed"
        >
          {loading ? 'Guardando...' : 'Guardar Evento'}
        </button>
      </form>
    </div>
  );
}
