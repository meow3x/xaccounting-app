import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import { QUERY_DEFAULT_STALE_TIME } from "src/defs";


export function useGetReceivableReport() {
  return useQuery({
    queryKey: ['report-receivable'],
    queryFn: async () => {
      const result = axios.get('/api/Reports/Accounts/Receivable')
      return (await result).data
    },
    staleTime: QUERY_DEFAULT_STALE_TIME
  })
}

export function useGetPayableReport() {
   return useQuery({
    queryKey: ['report-payable'],
    queryFn: async () => {
      const result = axios.get('/api/Reports/Accounts/Payable')
      return (await result).data
    },
    staleTime: QUERY_DEFAULT_STALE_TIME
  })
}