import { useMutation, useQuery } from "@tanstack/react-query";
import axios from "axios";
import { QUERY_DEFAULT_STALE_TIME } from "src/defs";


export function useGetInvoiceOnCredit() {
  return useQuery({
    queryKey: ['invoice-on-account' ],
    queryFn: async () => {
      const response = await axios.get('/api/Invoice/OnAccount')
      return response.data
    },
    staleTime: QUERY_DEFAULT_STALE_TIME
  })
}

export function useGetInvoiceOnCreditByNumber(invoiceNumber) {
  return useQuery({
    queryKey: ['invoice-on-account', invoiceNumber ],
    queryFn: async () => {
      const response = await axios.get('/api/Invoice/OnAccount/' + invoiceNumber)
      return response.data
    },
    enabled: !!invoiceNumber,
  })
}

export function usePostCollectorPayment() {
  return useMutation({
    mutationFn: async (request) => {
      const result = await axios.post('/api/Invoice/Payment', request)
      return result.data
    }
  })
}