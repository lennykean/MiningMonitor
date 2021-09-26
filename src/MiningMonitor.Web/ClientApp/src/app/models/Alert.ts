import { AlertActionResult } from './AlertActionResult';
import { AlertSeverity } from './AlertSeverity';

export interface Alert {
  id: string;
  minerId: string;
  alertDefinitionId: string;
  severity: AlertSeverity;
  message: string;
  start: Date;
  lastActive: Date;
  end: Date;
  acknowledgedAt: Date;
  acknowledged: boolean;
  active: boolean;
  detailMessages: string[];
  actionResults: AlertActionResult[];
}
