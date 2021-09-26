import { AlertActionDefinition } from './AlertActionDefinition';

export interface DisableGpuAlertActionDefinition extends AlertActionDefinition {
  disableAll: boolean;
  disableAffected: boolean;
}
