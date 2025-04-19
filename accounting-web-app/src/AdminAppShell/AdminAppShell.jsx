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
  IconUser, IconBuilding, IconUsers
} from '@tabler/icons-react';
import { Outlet, NavLink as ReactNavLink } from 'react-router';

export default function AdminAppShell() {
  const [opened, { toggle }] = useDisclosure();

  return (
    <AppShell
      header={{ height: 50 }}
      navbar={{ width: 300, breakpoint: 'sm', collapsed: { mobile: !opened } }}
      padding="md">
      <AppShell.Header>
        <Group h="100%" px="md">
          <Burger opened={opened} onClick={toggle} hiddenFrom="sm" size="sm" />
          <MantineLogo size={30} />
        </Group>
      </AppShell.Header>

      <title>Administration</title>

      <AppShell.Navbar p="md" >
        <ScrollArea>
          <NavLink
            label="Maintenance"
            leftSection={<IconSettingsCog size={16} />}
            childrenOffset={28}
            defaultOpened>
            <NavLink
              leftSection={<IconBook size={16} />}
              component={ReactNavLink}
              label="Chart of Account"
              to="chart-of-account" />
            <NavLink
              leftSection={<IconInvoice size={16} />}
              component={ReactNavLink}
              label="Items"
              to="items" />
            <NavLink
              leftSection={<IconUser size={16} />}
              component={ReactNavLink}
              label="Customers"
              to="customers" />
            <NavLink
              leftSection={<IconBuilding size={16} />}
              component={ReactNavLink}
              label="Suppliers"
              to="suppliers" />
            <NavLink
              leftSection={<IconUsers size={16} />}
              component={ReactNavLink}
              label="Employees"
              to="employees" />
          </NavLink>
        </ScrollArea>
      </AppShell.Navbar>

      <AppShell.Main>
        <Outlet />
      </AppShell.Main>

    </AppShell>
  );
}