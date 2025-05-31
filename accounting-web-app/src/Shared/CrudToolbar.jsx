import {Button, Group, Text} from "@mantine/core";
import {IconFileImport, IconPlus, IconUpload} from "@tabler/icons-react";

export default function CrudToolbar({title, onCreate, onImport}) {
  return (
    <Group justify="left" gap={4} mb={30}>
      <Text size="lg" c="dark.4" fw={600}>{title}</Text>
      <Button
        variant="subtle"
        leftSection={<IconPlus size={16} />}
        onClick={onCreate}
      >
        New
      </Button>

      {onImport?
        <Button
          size="sm"
          variant="subtle"
          leftSection={<IconUpload size={14}/>}
        >
          Import
        </Button>
        : <></>
      }
    </Group>
  )
}