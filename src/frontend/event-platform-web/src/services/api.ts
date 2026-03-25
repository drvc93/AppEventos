import axios from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5010';

const DEMO_JWT_TOKEN = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhZG1pbkBldmVudHBsYXRmb3JtLmNvbSIsIm5hbWUiOiJBZG1pbiIsInJvbGUiOiJBZG1pbiIsImlzcyI6IkV2ZW50UGxhdGZvcm0iLCJhdWQiOiJFdmVudFBsYXRmb3JtQ2xpZW50IiwiZXhwIjo0MTAyNDQ0ODAwfQ';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

api.interceptors.request.use((config) => {
  config.headers.Authorization = `Bearer ${DEMO_JWT_TOKEN}`;
  return config;
});

export interface CreateZoneRequest {
  name: string;
  price: number;
  capacity: number;
}

export interface CreateEventRequest {
  name: string;
  date: string;
  location: string;
  zones: CreateZoneRequest[];
}

export interface ZoneResponse {
  id: string;
  name: string;
  price: number;
  capacity: number;
}

export interface EventResponse {
  id: string;
  name: string;
  date: string;
  location: string;
  status: string;
  createdAt: string;
  zones: ZoneResponse[];
}

export const eventApi = {
  createEvent: (data: CreateEventRequest) =>
    api.post<EventResponse>('/api/events', data),

  getEvents: () =>
    api.get<EventResponse[]>('/api/events'),

  getEventById: (id: string) =>
    api.get<EventResponse>(`/api/events/${id}`),
};

export default api;
