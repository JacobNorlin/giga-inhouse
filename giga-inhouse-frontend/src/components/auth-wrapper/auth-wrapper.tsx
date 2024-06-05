import {
  AuthContextProvider
} from "@giga-inhouse/components/auth-wrapper/auth-context";
import { QueryWrapper } from "@giga-inhouse/components/query-wrapper/query-wrapper";
import { useGigaInhouseUser } from "@giga-inhouse/hooks/use-giga-inhouse-user";
import React from "react";

type AuthWrapperProps = React.PropsWithChildren;

export function AuthWrapper({ children }: AuthWrapperProps) {
  const userQuery = useGigaInhouseUser();

  return (
    <QueryWrapper queryStates={[userQuery]}>
      {(user) => {
        return (
          <AuthContextProvider user={user}>{children}</AuthContextProvider>
        );
      }}
    </QueryWrapper>
  );
}
