import { useGigaInhouseApi } from "@giga-inhouse/hooks/use-giga-inhouse-api";
import { useMutation, useQueryClient } from "@tanstack/react-query";

type MutateUserOptions = {
  userName?: string;
  steamId?: string;
};

export function useMutateGigaInhouseUser() {
  const api = useGigaInhouseApi();
  const queryClient = useQueryClient();

  const mutateUser = useMutation({
    mutationFn: async ({ userName, steamId }: MutateUserOptions) => {
      await api.request({
        method: "POST",
        url: "/Profile",
        data: {
          userName,
          steamId,
        },
      });
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["giga-inhouse", "user"] });
    },
  });

  return mutateUser;
}
