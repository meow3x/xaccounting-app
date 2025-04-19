import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import axios from 'axios'

import '@mantine/core/styles.layer.css'
import '@mantine/notifications/styles.css'
import 'mantine-datatable/styles.layer.css'
import './index.css'
import App from "src/App.jsx";

// Base API url
const CONFIG = import.meta.env
axios.defaults.baseURL = CONFIG.VITE_API_BASE_URL

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <App />
  </StrictMode>,
)
