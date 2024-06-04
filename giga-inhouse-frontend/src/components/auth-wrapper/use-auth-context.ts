import { AuthContext } from "@giga-inhouse/components/auth-wrapper/auth-context";
import React from "react";

export function useAuthContext() {
  const context = React.useContext(AuthContext);
  if (context === null) {
    throw Error("AuthContext not initialized");
  }

  return context;
}
