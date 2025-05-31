import { AppShell, Burger, Group, NavLink, ScrollArea} from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import { MantineLogo } from '@mantinex/mantine-logo';
import {
  IconList,
  IconSettingsCog,
  IconBook,
  IconBriefcase,
  IconFile3d,
  IconInvoice,
  IconUser, IconBuilding, IconUsers,
  IconCurrency,
  IconCurrencyDollar,
  IconBookFilled
} from '@tabler/icons-react';
import { Outlet, NavLink as ReactNavLink } from 'react-router';

export default function AdminAppShell() {
  const [opened, { toggle }] = useDisclosure();

  return (
    <AppShell
      // header={{ height: 50 }}
      navbar={{ width: 250, breakpoint: 'sm', collapsed: { mobile: !opened } }}
      padding="md">
      {/*<AppShell.Header>*/}
      {/*  <Group h="100%" px="md">*/}
      {/*    <Burger opened={opened} onClick={toggle} hiddenFrom="sm" size="sm" />*/}
      {/*    <MantineLogo size={30} />*/}
      {/*  </Group>*/}
      {/*</AppShell.Header>*/}

      <title>Administration</title>

      <AppShell.Navbar p="md" style={ { backgroundColor: 'var(--mantine-color-gray-1)', color: 'var(--mantine-color-dark-3)' }}>
        <ScrollArea fw={450}>
          <NavLink
            label="Maintenance"
            // rightSection={<IconSettingsCog size={16} />}
            defaultOpened>
            <NavLink
              // leftSection={<IconBook size={16} />}
              component={ReactNavLink}
              label="Chart of Account"
              to="chart-of-account" />
            <NavLink
              // leftSection={<IconInvoice size={16} />}
              component={ReactNavLink}
              label="Items"
              to="items" />
            <NavLink
              // leftSection={<IconUser size={16} />}
              component={ReactNavLink}
              label="Customers"
              to="customers" />
            <NavLink
              // leftSection={<IconBuilding size={16} />}
              component={ReactNavLink}
              label="Suppliers"
              to="suppliers" />
            <NavLink
              // leftSection={<IconUsers size={16} />}
              component={ReactNavLink}
              label="Employees"
              to="employees" />
          </NavLink>

          <NavLink
            label="Vendor & Purchases"
            // rightSection={<IconInvoice size={16} />}
            defaultOpened
          >
            <NavLink
              component={ReactNavLink}
              label="Create Purchase Order"
              to="purchase-order" />

            <NavLink
              component={ReactNavLink}
              label="Receive Purchase Order"
              to="receive-purchase-order" />
          </NavLink>

          <NavLink
            label="Journal Entries"
            // leftSection={<IconBook size={16} />}
            defaultOpened
          >
            <NavLink
              component={ReactNavLink}
              label="Payable"
              to="accounts-payable" />

            <NavLink
              component={ReactNavLink}
              label="Disbursement"
              to="disbursements" />


            <NavLink
              component={ReactNavLink}
              label="General"
              to="general-journal" />
          </NavLink>

          <NavLink
            component={ReactNavLink}
            label="Inventory"
            to="inventory" />
        </ScrollArea>
      </AppShell.Navbar>

      <AppShell.Main>
        <Outlet />
      </AppShell.Main>

    </AppShell>
  );
}