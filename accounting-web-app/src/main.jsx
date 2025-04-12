import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Route, Routes } from 'react-router'
import { createTheme, MantineProvider } from '@mantine/core'
import { Notifications } from '@mantine/notifications'
import axios from 'axios'
import CustomerList from "src/Customers/CustomerList.jsx";
import SupplierList from "src/Suppliers/SupplierList.jsx";
import AdminAppShell from 'src/AdminAppShell/AdminAppShell.jsx';
import AccountList from 'src/ChartOfAccount/AccountList.jsx'
import ItemList from './Items/ItemList.jsx';

// import '@mantine/core/styles.css'
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
