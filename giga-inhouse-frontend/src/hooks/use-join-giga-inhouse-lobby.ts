import { useGigaInhouseApi } from "@giga-inhouse/hooks/use-giga-inhouse-api";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useNavigate } from "react-router";

export function useJoinGigaInhouseLobby() {
  const api = useGigaInhouseApi();
  const navigate = useNavigate();
  const queryClient = useQueryClient();


  const joinLobby = useMutation({
    mutationFn: async ({ lobbyId }: { lobbyId: string | undefined }) => {
      await api.request({
        url: "/Lobby/Join",
        method: "POST",
        params: {
          lobbyId,
        },
      });
    },
    onSettled: (_d, _err, { lobbyId }) => {
      queryClient.invalidateQueries({queryKey: ["giga-inhouse", "lobby", lobbyId]})
      navigate(`/lobby/${lobbyId}`);
    },
  });

  return joinLobby;
}
