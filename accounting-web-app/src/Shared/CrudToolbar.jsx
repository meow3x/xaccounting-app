import {Button, Group} from "@mantine/core";
import {IconFileImport, IconPlus} from "@tabler/icons-react";

export default function CrudToolbar({onCreate}) {
  return (
    <Group justify="left" gap={4} mb={30}>
      <Button
        variant="filled"
        rightSection={<IconPlus size={16} />}
        onClick={onCreate}
      >
        New
      </Button>
      <Button variant="outline" rightSection={<IconFileImport size={16} />}>
        Import
      </Button>
    </Group>
  )
}