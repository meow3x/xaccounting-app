import {useEffect, useState} from "react";
import {PAGE_SIZES} from "src/util/table/common.jsx";

export function usePaginationState(pageSizes) {
  const [pageSize, setPageSize] = useState(pageSizes[0]);
  const [page, setPage] = useState(1);

  useEffect(() => {
    setPage(1);
  }, [pageSize]);

  return {
    recordsPerPage: pageSize,
    page: page,
    onPageChange: setPage,
    recordsPerPageOptions: PAGE_SIZES,
    onRecordsPerPageChange: setPageSize
  };
}