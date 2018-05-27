import { addTask } from 'domain-task';

export function makeDomainTask<T>(task: Promise<T>) {
    addTask(task);
    return task;
}
