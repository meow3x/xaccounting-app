import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Route, Routes } from 'react-router'
import { createTheme, MantineProvider } from '@mantine/core'
import { Notifications } from '@mantine/notifications'
import AdminAppShell from './AdminAppShell/AdminAppShell.jsx';
import CoaIndex from './ChartOfAccount/CoaIndex.jsx'
import AccountTypesIndex from './AccountTypes/AccountTypesIndex.jsx';

import './App.css'
import './index.css'
import '@mantine/core/styles.css'

const theme = createTheme({
  fontFamily: 'Roboto Mono, monospace'
})

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <MantineProvider theme={theme} defaultColorScheme="dark">
      <Notifications />

      <BrowserRouter>
        <Routes>
          <Route path="/" element={<AdminAppShell />}>
            <Route path="account-types" element={<AccountTypesIndex />} />
            <Route path="chart-of-account" element={<CoaIndex />}/>
          </Route>
        </Routes>
      </BrowserRouter>
    </MantineProvider>
  </StrictMode>,
)
