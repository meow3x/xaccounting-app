import {ActionIcon, MultiSelect, TextInput} from "@mantine/core";
import {IconSearch, IconX} from "@tabler/icons-react";

/**
 * Text input with search and clear icon
 * @returns {JSX.Element}
 * @constructor
 */
export function TextFilter({label, query, setQuery}) {
  return (
    <TextInput
      label={label}
      placeholder={`Input ${label}`}
      leftSection={<IconSearch size={16} />}
      rightSection={
        // clear
        <ActionIcon size="sm" variant="transparent" c="dimmed" onClick={() => setQuery('')} >
          <IconX size={14} />
        </ActionIcon>
      }
      value={query}
      onChange={e => setQuery(e.currentTarget.value)} />
  )
}

export function MultiSelectFilter({label, data, value, onChange}) {
  return (
    <MultiSelect
      label={label}
      placeholder={`Input ${label}`}
      data={data}
      value={value}
      onChange={onChange}
      leftSection={<IconSearch size={16}/>}
      comboboxProps={{ withinPortal: false }}
      clearable
      searchable />
  )
}