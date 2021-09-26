import { AlertActionType } from './AlertActionType';

export interface AlertActionDefinition {
  displayName?: string;
  type: AlertActionType;
  name: string;
}
