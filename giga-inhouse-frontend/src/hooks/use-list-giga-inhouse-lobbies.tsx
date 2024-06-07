import { useGigaInhouseApi } from "@giga-inhouse/hooks/use-giga-inhouse-api";
import { Lobby } from "@giga-inhouse/hooks/use-giga-inhouse-lobby";
import { useQuery } from "@tanstack/react-query";



export function useListGigaInhouseLobbies(){
  const api = useGigaInhouseApi();

  const lobbiesQuery = useQuery({
    queryKey: ["giga-inhouse", "lobbies"],
    queryFn: async () => {
      const res = await api.get<Lobby[]>("/Lobby/List");

      return res.data;
    }
  })

  return lobbiesQuery;

}
