import { useQuery } from "@tanstack/react-query";
import axios from "axios";


export function useGetCostCenters() {
  return useQuery({
    queryKey: ['cost-centers'],
    queryFn: async () => {
      const result = await axios.get('/api/AccountsPayable/CostCenters')
      return result.data
    },
    staleTime: 300000
  })
}