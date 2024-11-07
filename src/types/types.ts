export interface ReportEntry {
  comment: string
  duration: number
}

export interface ReportVM {
  headline: string
  entries: ReportEntry[]
  total: string
}
