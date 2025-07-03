import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import { QUERY_DEFAULT_STALE_TIME } from "src/defs";

export function useInventoryQuantityView() {
  return useQuery({
    queryKey: ['inventory-quantity'],
    queryFn: async () => {
      const result = await axios.get('/api/Inventory/Views/Quantity')
      return result.data
    },
    staleTime: 300000
  })
}

export function useGetItemInventoryQty(itemId) {
  return useQuery({
    queryKey: ['inventory-quantity', itemId ],
    queryFn: async () => {
        const result = await axios.get('/api/Inventory/Views/Quantity/' + itemId)
        return result.data
    },
    enabled: !!itemId
  })
}

export function useInventoryCostView() {
  return useQuery({
    queryKey: ['inventory-cost'],
    queryFn: async () => {
      const result = await axios.get('/api/Inventory/Views/EndingCost')
      return result.data
    },
    staleTime: 300000
  })
}
