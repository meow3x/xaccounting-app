import { ActionIcon, Box, Group } from "@mantine/core";
import { IconEdit, IconEye, IconSettingsCog, IconTrash } from "@tabler/icons-react";
import { DataTable } from "mantine-datatable";
import { useState, useEffect } from "react";
import axios from "axios";

export const PAGE_SIZES = [10, 20, 50, 100];

export function usePaginationState(pageSizes) {
  const [pageSize, setPageSize] = useState(pageSizes[0]);
  const [page, setPage] = useState(1);

  useEffect(() => { setPage(1); }, [pageSize]);

  return {
    recordsPerPage: pageSize,
    page: page,
    onPageChange: setPage,
    recordsPerPageOptions: PAGE_SIZES,
    onRecordsPerPageChange: setPageSize
  };
}

export function DataTableWrapper({
                                   columns,
                                   records,
                                   paginationState,
                                   totalRecords,
                                   isFetching}) {
  // const paginationProps = usePaginationDefaults(PAGE_SIZES);

  return (
    <DataTable
      striped
      highlightOnHover
      minHeight={200}
      columns={columns}
      records={records}
      totalRecords={totalRecords}
      fetching={isFetching}
      {...paginationState}
    />
  );
}

export function Peso(value) {
  return (
    <>{typeof(value) === 'number'
      ? <>&#8369; {value.toFixed(2)} </>
      : '-' }
    </>
  )
}

export function Percentage(value) {
  return (
    <>{typeof(value) === 'number'
      ? <>{value.toFixed(2)} %</>
      : '-' }
    </>
  )
}

export function Opt(value) {
  return value == null ? '' : value
}

export function DataTableRowActions({ onEditClick, onDeleteClick, onViewClick }) {
  return (
    <Group gap={4}  wrap="nowrap">
      <ActionIcon size="sm" variant="subtle" color="green" onClick={onViewClick}>
        <IconEye size={16} />
      </ActionIcon>
      <ActionIcon size="sm" variant="subtle" color="blue" onClick={onEditClick}>
        <IconEdit size={16} />
      </ActionIcon>
      <ActionIcon size="sm" variant="subtle" color="red" onClick={onDeleteClick}>
        <IconTrash size={16} />
      </ActionIcon>
    </Group>
  )
}