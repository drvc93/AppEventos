import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';
import EventList from './pages/EventList';
import CreateEvent from './pages/CreateEvent';

function App() {
  return (
    <BrowserRouter>
      <div className="min-h-screen bg-gray-100">
        <nav className="bg-white shadow-sm border-b">
          <div className="max-w-4xl mx-auto px-6 py-3 flex items-center gap-6">
            <Link to="/" className="text-xl font-bold text-blue-600">
              Plataforma de Eventos
            </Link>
            <Link to="/" className="text-gray-600 hover:text-gray-800 text-sm">
              Eventos
            </Link>
            <Link to="/create" className="text-gray-600 hover:text-gray-800 text-sm">
              Registrar
            </Link>
          </div>
        </nav>

        <main className="py-6">
          <Routes>
            <Route path="/" element={<EventList />} />
            <Route path="/create" element={<CreateEvent />} />
          </Routes>
        </main>
      </div>
    </BrowserRouter>
  );
}

export default App;
