import { AppShell, Burger, Group, Skeleton, NavLink, ScrollArea, Text} from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import { MantineLogo } from '@mantinex/mantine-logo';
import { IconGauge, IconFingerprint, IconTools, IconList, IconEyeCog, IconSettings2, IconSettings, IconSettingsCog } from '@tabler/icons-react';
import { Outlet, NavLink as ReactNavLink } from 'react-router';
import { Notifications } from '@mantine/notifications';

export default function AdminAppShell() {
  const [opened, { toggle }] = useDisclosure();

  return (
    <AppShell
      header={{ height: 60 }}
      navbar={{ width: 300, breakpoint: 'sm', collapsed: { mobile: !opened } }}
      padding="md"
    >
      <AppShell.Header>
        <Group h="100%" px="md">
          <Burger opened={opened} onClick={toggle} hiddenFrom="sm" size="sm" />
          <MantineLogo size={30} />
        </Group>
      </AppShell.Header>

      <AppShell.Navbar p="md">
        <ScrollArea>
          <NavLink
            label="Maintenance"
            leftSection={<IconSettingsCog size={16} />}
            childrenOffset={28}
            defaultOpened
          >
          <NavLink leftSection={<IconList size={16} />} component={ReactNavLink} label="Chart of Account" to="chart-of-account"/>
          <NavLink leftSection={<IconList size={16} />} component={ReactNavLink} label="Account Types" to="account-types"/>

          </NavLink>
        </ScrollArea>
      </AppShell.Navbar>

      <AppShell.Main>
        <Outlet />
      </AppShell.Main>

    </AppShell>
  );
}